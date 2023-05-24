using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    Node[,] grid;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
	public int obstacleProximityPenalty = 10;
	Dictionary<int,int> walkableRegionsDictionary = new Dictionary<int, int>();
	LayerMask walkableMask;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
	int penaltyMax = int.MinValue;
    public int MaxSize {
        get => gridSizeX*gridSizeY;
    }

    void Awake(){
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        foreach (TerrainType region in walkableRegions) {
			walkableMask.value |= region.terrainMask.value;
			walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
		}

        CreateGrid();
    }

    public List<Node> GetNeighbours(Node node){
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x<=1;x++){
            for (int y =-1; y<=1;y++){
                if (x==0 && y==0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX>=0 && checkX < gridSizeX && checkY>=0 && checkY < gridSizeY){
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector2 WorldPosition){
        float percentX = (WorldPosition.x+gridWorldSize.x/2)/gridWorldSize.x;
        float percentY = (WorldPosition.y+gridWorldSize.y/2)/gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX-1)*percentX);
        int y = Mathf.RoundToInt((gridSizeY-1)*percentY);
        return grid[x,y]; 
    }

    void BlurPenaltyMap(int blurSize) {
		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1) / 2;

		int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
		int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

		for (int y = 0; y < gridSizeY; y++) {
			for (int x = -kernelExtents; x <= kernelExtents; x++) {
				int sampleX = Mathf.Clamp (x, 0, kernelExtents);
				penaltiesHorizontalPass [0, y] += grid [sampleX, y].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++) {
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX-1);

				penaltiesHorizontalPass [x, y] = penaltiesHorizontalPass [x - 1, y] - grid [removeIndex, y].movementPenalty + grid [addIndex, y].movementPenalty;
			}
		}
			
		for (int x = 0; x < gridSizeX; x++) {
			for (int y = -kernelExtents; y <= kernelExtents; y++) {
				int sampleY = Mathf.Clamp (y, 0, kernelExtents);
				penaltiesVerticalPass [x, 0] += penaltiesHorizontalPass [x, sampleY];
			}

			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, 0] / (kernelSize * kernelSize));
			grid [x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridSizeY; y++) {
				int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY-1);

				penaltiesVerticalPass [x, y] = penaltiesVerticalPass [x, y-1] - penaltiesHorizontalPass [x,removeIndex] + penaltiesHorizontalPass [x, addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass [x, y] / (kernelSize * kernelSize));
				grid [x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax) {
					penaltyMax = blurredPenalty;
				}
				if (blurredPenalty < penaltyMin) {
					penaltyMin = blurredPenalty;
				}
			}
		}

	}

    public void CreateGrid(){
        Vector2 worldBottomLeft = (Vector2) transform.position-Vector2.right*gridWorldSize.x/2 - Vector2.up*gridWorldSize.y/2;
        grid = new Node[gridSizeX,gridSizeY];
        
		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector2 worldPoint = worldBottomLeft + Vector2.right*(x*nodeDiameter+nodeRadius) + Vector2.up * (y*nodeDiameter+nodeRadius);
                RaycastHit2D cast = Physics2D.CircleCast(worldPoint,nodeRadius,Vector2.zero,0.1f,unwalkableMask);
                bool walkable = (cast.transform is null);

				int movementPenalty = 0;

				RaycastHit2D hit = Physics2D.Raycast(worldPoint, worldPoint,100,walkableMask);
				if (hit.collider is not null) {
					walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
				}

				if (!walkable) {
					movementPenalty += obstacleProximityPenalty;
				}


				grid[x,y] = new Node(walkable,worldPoint, x,y, movementPenalty);
			}
		}
        BlurPenaltyMap (3);
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y));
    }

    [System.Serializable]
	public class TerrainType {
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}

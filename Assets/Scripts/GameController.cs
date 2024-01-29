using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameData gameData;
    public Canvas UI;
    public TextMeshProUGUI MatchUI;
    public GameObject FailMenu;

    Dictionary<int, int> count = new Dictionary<int, int>();

    private GameObject gridPrefab;
    private GridConfig gridPrefabConfig;

    Tile firsTile;

    public Tile[,,] TilesArray;
    List<TileData> TileTypeArray = new List<TileData>();
    int indexX;
    int indexY;
    int indexZ;



    // Start is called before the first frame update
    void Start()
    {
        TileEvent.TileClikedEvent += TileMatchCheck;
        NewGame();

    }

    public void NewGame()
    {        
        if (gridPrefab != null)
        {
            Destroy(gridPrefab.gameObject);
            gridPrefabConfig = null;
            TileTypeArray.Clear();
        }
        FailMenu.SetActive(false);
        InstantiateGrid();
        GridInit();

    }

    //загружаем префаб сетки на сцену
    void InstantiateGrid()
    {
        int randomIndex = Random.Range(0, gameData.gridsPrefabs.Count);
        gridPrefab = Instantiate(gameData.gridsPrefabs[randomIndex]);
        gridPrefabConfig = gridPrefab.GetComponent<GridConfig>();
    }

    //заполняем массив плитками сетки и задаем спрайтам и эффектам уровень отрисовки
    void GridInit()
    {
        int tileXIndex;
        int tileYIndex;
        int tileZIndex;

        TilesArray = new Tile[(int)gridPrefabConfig.GridSize.x, (int)gridPrefabConfig.GridSize.y, (int)gridPrefabConfig.GridSize.z];

        for (int i = 0; i < gridPrefabConfig.Tiles.Count; i++)
        {

            tileXIndex = (int)gridPrefabConfig.Tiles[i].tilePos.x;
            tileYIndex = (int)gridPrefabConfig.Tiles[i].tilePos.y;
            tileZIndex = (int)gridPrefabConfig.Tiles[i].tilePos.z;

            TilesArray[tileXIndex, tileYIndex, tileZIndex] = gridPrefabConfig.Tiles[i];
            TilesArray[tileXIndex, tileYIndex, tileZIndex].orderInLayer = tileZIndex * 100 + tileXIndex * 10 - tileYIndex;

        }

        for (int i = 0; i < gridPrefabConfig.Tiles.Count / gridPrefabConfig.numberOfType; i++)
        {
            for (int j = 0; j < gridPrefabConfig.numberOfType; j++)
            {
                TileTypeArray.Add(gameData.tileTypes[i]);
            }
        }

        ParsTileType();

        GridUpdate();
        CountUpdate();
    }

    void ParsTileType()
    {
        int randomIndex;
        for (int i = 0; i < TileTypeArray.Count; i++)
        {
            do
            {
                randomIndex = Random.Range(0, gridPrefabConfig.Tiles.Count);
            }
            while (gridPrefabConfig.Tiles[randomIndex].tileType != null);
            DefineTileType(gridPrefabConfig.Tiles[randomIndex], TileTypeArray[i]);
            SetLayers(gridPrefabConfig.Tiles[randomIndex]);
        }
    }

    public void RefreshTileType()
    {
        foreach (Tile item in gridPrefabConfig.Tiles)
        {
            item.tileType = null;
            item.IsSelected = false;
            firsTile = null;
        }
        ParsTileType();
        FailMenu.SetActive(false);
        CountUpdate();
    }

    void DefineTileType(Tile item, TileData data)
    {
        if (item.SelectVFX != null)
        {
            Destroy(item.SelectVFX.gameObject);
        }

        item.tileType = data;
        item.ID = data.tileTypeID;
        item.tileTypeSprite.sprite = data.tileSprite;
        item.SelectVFX = Instantiate(data.tileTypeVFX, item.VFX.transform);
        item.VFXRenderer = item.SelectVFX.GetComponent<Renderer>();
        item.SelectVFX.gameObject.SetActive(false);
    }
    void SetLayers(Tile item)
    {
        item.tileBackSprite.sortingOrder = item.orderInLayer;
        item.tileTypeSprite.sortingOrder = item.orderInLayer + 1;
        item.shadowSprite.sortingOrder = item.orderInLayer - 1;
        item.VFXRenderer.sortingOrder = item.orderInLayer + (int)gridPrefabConfig.GridSize.x + (int)gridPrefabConfig.GridSize.y;
    }

    //находим заблокированные плитки
    void GridUpdate()
    {
        foreach (Tile item in TilesArray)
        {

            if (item != null)
            {


                if (IsTopCover(item) || IsSideCover(item))
                {
                    item.tileBackSprite.color = gameData.BlockedTileColor;
                    item.IsBloced = true;
                }
                else
                {
                    item.tileBackSprite.color = Color.white;
                    item.IsBloced = false;
                }

            }

        }

    }

    //проверяем перекрыта ли плитка сверху
    bool IsTopCover(Tile item)
    {
        indexZ = (int)item.tilePos.z + 1;
        if (indexZ >= gridPrefabConfig.GridSize.z)
        {
            return false;
        }
        for (int j = -1; j < 2; j++)
        {
            for (int i = -1; i < 2; i++)
            {
                indexX = (int)item.tilePos.x + i;
                indexY = (int)item.tilePos.y + j;

                if (indexX >= 0 && indexX < gridPrefabConfig.GridSize.x &&
                    indexY >= 0 && indexY < gridPrefabConfig.GridSize.y &&
                    TilesArray[indexX, indexY, indexZ] != null)
                {
                    if (Mathf.Abs(item.transform.position.x - TilesArray[indexX, indexY, indexZ].transform.position.x) < item.tileBackSprite.bounds.size.x - gridPrefabConfig.OfsetX &&
                        Mathf.Abs(item.transform.position.y - TilesArray[indexX, indexY, indexZ].transform.position.y) < item.tileBackSprite.bounds.size.y - gridPrefabConfig.OfsetY)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //проверяем перекрыта ли плитка с обеих сторон
    bool IsSideCover(Tile item)
    {
        indexZ = (int)item.tilePos.z;

        int lefSide = 0;
        int rightSide = 0;

        for (int i = -1; i < 2; i++)
        {
            if (i == 0)
            {
                continue;
            }
            indexX = (int)item.tilePos.x + i;
            if (indexX < 0 | indexX >= gridPrefabConfig.GridSize.x)
            {
                return false;
            }

            for (int j = -1; j < 2; j++)
            {
                indexY = (int)item.tilePos.y + j;

                if (indexX >= 0 && indexX < gridPrefabConfig.GridSize.x &&
                    indexY >= 0 && indexY < gridPrefabConfig.GridSize.y &&
                    TilesArray[indexX, indexY, indexZ] != null)
                {
                    if (Mathf.Abs(item.transform.position.y - TilesArray[indexX, indexY, indexZ].transform.position.y) < item.tileBackSprite.bounds.size.y - gridPrefabConfig.OfsetY)
                    {
                        if (i < 0)
                        {
                            lefSide++;
                        }
                        else if (i > 0)
                        {
                            rightSide++;
                        }
                    }
                }

            }
        }
        if (lefSide > 0 && rightSide > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CountUpdate()
    {
        count.Clear();
        int temp = 0;
        foreach (Tile item in gridPrefabConfig.Tiles)
        {
            if (!item.IsBloced)
            {
                if (count.ContainsKey(item.ID))
                {
                    count[item.ID]++;
                }
                else
                {
                    count[item.ID] = 1;
                }
            }
        }
        foreach (var pair in count)
        {
            temp += pair.Value / 2;
        }
        MatchUI.text = "Matches: " + temp;
        if (temp == 0)
        {
            FailMenu.SetActive(true);
        }
    }

    void SelectCheck(Tile item)
    {
        if (!item.IsSelected)
        {
            item.IsSelected = true;
            item.SelectVFX.gameObject.SetActive(true);
        }
        else
        {
            item.IsSelected = false;
            item.SelectVFX.gameObject.SetActive(false);
        }
    }
    void TileMatchCheck(Tile item)
    {
        if (firsTile == null)
        {
            firsTile = item;
            SelectCheck(item);
        }
        else if (firsTile == item)
        {
            firsTile = null;
            SelectCheck(item);
        }
        else
        {
            if (firsTile.ID == item.ID && firsTile != item)
            {
                TileTypeArray.Remove(item.tileType);
                TileTypeArray.Remove(firsTile.tileType);

                gridPrefabConfig.Tiles.Remove(item);
                gridPrefabConfig.Tiles.Remove(firsTile);



                Destroy(item.gameObject);
                Destroy(firsTile.gameObject);
                TilesArray[(int)item.tilePos.x, (int)item.tilePos.y, (int)item.tilePos.z] = null;
                TilesArray[(int)firsTile.tilePos.x, (int)firsTile.tilePos.y, (int)firsTile.tilePos.z] = null;
                firsTile = null;
                GridUpdate();
                CountUpdate();
            }
            else
            {
                SelectCheck(firsTile);

                SelectCheck(item);

                firsTile = item;
            }
        }
    }
}

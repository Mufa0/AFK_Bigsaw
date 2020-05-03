using UnityEngine;
using System.Linq;
using System.IO;

public class GameController : MonoBehaviour
{

    public GameObject playScreen;

    private PuzzlePieceController[] non_edge_pieces;
    private PuzzlePieceController[] edge_pieces;
    // Start is called before the first frame update

    private bool onlyEdgesVisible = false;

    public static GameObject newPuzzlePrefab = null;
    public static bool rotation = false;

    void Start()
    {

        if (EventManager.playScreenEnabled == null)
        {
            EventManager.playScreenEnabled = new PlayScreenEvent();
        }
        EventManager.playScreenEnabled.AddListener(PlayScreenEnabled);

        PuzzlePieceController[] pieces = playScreen.GetComponentsInChildren<PuzzlePieceController>(true);
        non_edge_pieces = pieces.Where(x => !x.gameObject.CompareTag("Edge")).ToArray();
        edge_pieces = pieces.Where(x => x.gameObject.CompareTag("Edge")).ToArray();

    }

    public void setNewPuzzlePrefab(GameObject puzzle)
    {
        newPuzzlePrefab = puzzle;
    }
    public void allowRotation()
    {
        rotation = !rotation;
    }

    private void PlayScreenEnabled(bool enabled)
    {
        if (enabled)
        {
            
            PuzzlePieceController[] pieces = playScreen.GetComponentsInChildren<PuzzlePieceController>(true);
            non_edge_pieces = pieces.Where(x => !x.gameObject.CompareTag("Edge")).ToArray();
            edge_pieces = pieces.Where(x => x.gameObject.CompareTag("Edge")).ToArray();
        }
        else
        {
            
            edge_pieces = null;
            non_edge_pieces = null;
        }
       
    }

    private void OnlyEdges()
    {
        
        foreach (PuzzlePieceController go in non_edge_pieces)
        {
            go.gameObject.SetActive(false);
        }
        onlyEdgesVisible = true;
    }

    private void ResetVisibility()
    {
        foreach (PuzzlePieceController go in non_edge_pieces)
        {
            go.gameObject.SetActive(true);
            
        }
        onlyEdgesVisible = false;
    }

    public void ChangeVisibilityController()
    {
        if (onlyEdgesVisible)
        {
            ResetVisibility();
        }
        else
        {
            OnlyEdges();
        }
    }

    public void SaveGame()
    {
        Save save = new Save();

        PuzzlePieceController[] puzzlePieces = playScreen.GetComponentsInChildren<PuzzlePieceController>().Where(item => item.gameObject.layer.Equals(LayerMask.NameToLayer("Clickable"))).ToArray();
        foreach (PuzzlePieceController piece in puzzlePieces)
        {
            float[] position = { piece.transform.position.x, piece.transform.position.y, piece.transform.position.z };
            float[] rotation = { piece.transform.rotation.x, piece.transform.rotation.y, piece.transform.rotation.z, piece.transform.rotation.w };
            save.puzzles.Add(new Puzzle(piece.name, position, rotation));
        }

        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        string filename = Application.persistentDataPath + "/savegame.json";
        FileStream fileStream = File.Create(filename);
        fileStream.Close();

        File.WriteAllText(filename, json);

       

    }

    public void LoadGame()
    {
        

        PuzzlePieceController[] puzzlePieces = playScreen.GetComponentsInChildren<PuzzlePieceController>().Where(item => item.gameObject.layer.Equals(LayerMask.NameToLayer("Clickable"))).ToArray();

        string filename = Application.persistentDataPath + "/savegame.json";

        string json = File.ReadAllText(filename);

        Save save = JsonUtility.FromJson<Save>(json);

        foreach(PuzzlePieceController piece in puzzlePieces)
        {
            Puzzle puzzle = save.puzzles.Where(t => t.name.Equals(piece.name)).First();
            piece.transform.position = new Vector3(puzzle.position[0], puzzle.position[1], puzzle.position[2]);
            piece.transform.rotation = new Quaternion(puzzle.rotation[0], puzzle.rotation[1], puzzle.rotation[2], puzzle.rotation[3]);
        }

        Debug.Log("Application Loaded!");


    }

}

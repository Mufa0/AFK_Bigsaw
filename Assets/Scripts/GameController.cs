using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

}

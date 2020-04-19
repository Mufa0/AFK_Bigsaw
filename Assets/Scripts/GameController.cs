using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameController: MonoBehaviour
{

    public GameObject playScreen;

    private PuzzlePieceController[] non_edge_pieces;
    private PuzzlePieceController[] edge_pieces;
    // Start is called before the first frame update

    private bool onlyEdgesVisible = false;
    void Start()
    {
        PuzzlePieceController[] pieces = playScreen.GetComponentsInChildren<PuzzlePieceController>();
        non_edge_pieces = pieces.Where(x => !x.gameObject.CompareTag("Edge")).ToArray();
        edge_pieces = pieces.Where(x => x.gameObject.CompareTag("Edge")).ToArray();

    }

    private void OnEnable()
    {
        PuzzlePieceController[] pieces = playScreen.GetComponentsInChildren<PuzzlePieceController>();
        non_edge_pieces = pieces.Where(x => !x.gameObject.CompareTag("Edge")).ToArray();
        edge_pieces = pieces.Where(x => x.gameObject.CompareTag("Edge")).ToArray();
    }

    private void OnlyEdges()
    {
        foreach (PuzzlePieceController go in non_edge_pieces)
        {
            go.gameObject.SetActive(false);
            onlyEdgesVisible = true;
        }
    }

    private void ResetVisibility()
    {
        foreach(PuzzlePieceController go in non_edge_pieces)
        {
            go.gameObject.SetActive(true);
            onlyEdgesVisible = false;
        }
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

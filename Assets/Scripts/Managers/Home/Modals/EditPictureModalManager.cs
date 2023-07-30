
using UnityEngine;
using UnityEngine.UI;

public class EditPictureModalManager : Singleton<EditPictureModalManager>
{
    [SerializeField]
    private Transform content;

    [SerializeField]
    private Transform pictureCellPrefab;

    [SerializeField]
    private Button cancelButton;

    public int SelectedPictureIndex { get; set; } = 0;

    private void Start()
    {
        var pictureKvps = PathUtility.LoadAllIndexedPictures();

        foreach (var pictureKvp in pictureKvps)
        {
            var pictureCellInstance = Instantiate(pictureCellPrefab, content);

            pictureCellInstance.GetComponent<PictureCellManager>().SetAttributes(pictureKvp.Key);
            
            
        }
    }


}
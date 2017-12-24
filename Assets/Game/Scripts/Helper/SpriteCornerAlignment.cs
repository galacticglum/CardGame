using UnityEngine;
using UnityUtilities;

public class SpriteCornerAlignment : MonoBehaviour
{
    [SerializeField]
    private Corner corner;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private Sprite targetSprite;
    private Vector2? targetScale;

    private float lastHandledAspect;

    private void Update()
    {
        if (Camera.main.aspect == lastHandledAspect) return;
        Reposition();
    }

    public void Reposition()
    {
        lastHandledAspect = Camera.main.aspect;

        float orthographicWidth = Camera.main.orthographicSize * lastHandledAspect;
        float halfWidthInUnits = targetSprite.texture.width / targetSprite.pixelsPerUnit / 2;

        Vector2 size = targetScale ?? transform.localScale;

        Vector3 position = transform.position;
        switch (corner)
        {
            case Corner.TopLeft:
                float xOffset = halfWidthInUnits * size.x - halfWidthInUnits;
                float yOffset = targetSprite.texture.height / targetSprite.pixelsPerUnit / 2 * size.y;

                position = new Vector3(-orthographicWidth - xOffset, Camera.main.orthographicSize + yOffset, 0) + (Vector3)offset;
                break;
            case Corner.TopRight:
                xOffset = halfWidthInUnits * size.x - halfWidthInUnits * 2;
                yOffset = targetSprite.texture.height / targetSprite.pixelsPerUnit / 2 * size.y;

                position = new Vector3(orthographicWidth + xOffset, -Camera.main.orthographicSize + yOffset, 0) + (Vector3)offset;
                break;
            case Corner.BottomLeft:
                xOffset = halfWidthInUnits * size.x - halfWidthInUnits;
                yOffset = targetSprite.texture.height / targetSprite.pixelsPerUnit / 2 * size.y;

                position = new Vector3(-orthographicWidth - xOffset, -Camera.main.orthographicSize + yOffset, 0) + (Vector3)offset;
                break;
            case Corner.BottomRight:
                xOffset = halfWidthInUnits * size.x - halfWidthInUnits * 2;
                yOffset = targetSprite.texture.height / targetSprite.pixelsPerUnit / 2 * size.y;

                position = new Vector3(orthographicWidth + xOffset, -Camera.main.orthographicSize + yOffset, 0) + (Vector3)offset;
                break;
        }

        position.z = 0;
        transform.position = position;
    }

    public static void Attach(GameObject gameObject, Corner corner, Sprite sprite, Vector2? size = null, Vector2? offset = null)
    {
        SpriteCornerAlignment alignment = gameObject.AddComponent<SpriteCornerAlignment>();
        alignment.corner = corner;
        alignment.offset = offset ?? Vector2.zero;
        alignment.targetScale = size;
        alignment.targetSprite = sprite;
    }
}
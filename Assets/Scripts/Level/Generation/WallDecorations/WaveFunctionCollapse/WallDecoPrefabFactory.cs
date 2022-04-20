using UnityEngine;

namespace Level.Generation.WallDecorations.WaveFunctionCollapse
{
    public class WallDecoPrefabFactory : MonoBehaviour
    {
        [SerializeField] private PaintingGenerator paintingGenerator;

        public GameObject Create(WallDecoration deco, Transform parent)
        {
            switch (deco.decoName)
            {
                case WallDecoName.PICTURE_FRAME:
                    GameObject painting = paintingGenerator.Generate(parent);
                    painting.transform.localPosition += deco.offset;
                    return painting;
                default: 
                    GameObject createdDeco = Instantiate(deco.prefab, parent);
                    createdDeco.transform.localPosition += deco.offset;
                    return createdDeco;
            }
        }
    }
}
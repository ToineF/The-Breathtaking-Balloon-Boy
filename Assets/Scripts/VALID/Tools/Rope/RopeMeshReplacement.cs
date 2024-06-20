using Obi;
using UnityEngine;
using AntoineFoucault.Utilities;

public class RopeMeshReplacement : MonoBehaviour
{
    [SerializeField] private MeshFilter _newRopePrefbab;
    [SerializeField] private Transform _parent;

    [ContextMenu("Replace")]
    public void ReplaceAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.GetComponent<ObiRope>() == false) continue;

            if (child.TryGetComponent(out MeshFilter meshFilter) == false) continue;

            Mesh mesh = meshFilter.sharedMesh;

            var newRope = Instantiate(_newRopePrefbab, child.transform.position, child.transform.rotation, _parent);
            newRope.mesh = mesh;
        }

      //  transform.Clear();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TextureSprite : MonoBehaviour
{
#region Texture and MeshUVs

    public Rect rect;               // UV情報
    public Texture texture = null; // テクスチャ

    static MaterialPropertyBlock block = null;
    static int shaderMainTexID;
    static List<Vector2> sharedUvs = new List<Vector2>(4);

    [SerializeField] public Mesh quadMesh;     // QUADオリジナル

    // 同一のUVを持つメッシュをキャッシュ <UV情報のハッシュ値, uv情報込のメッシュ>
    static Dictionary<int, Mesh> storedMesh = new Dictionary<int, Mesh>();

    /// <summary>
    // テクスチャ情報を更新
    /// </summary>
    public void UpdateTexture()
    {
        if (texture == null) return;

        if( block == null) {
            block = new MaterialPropertyBlock();
            shaderMainTexID = Shader.PropertyToID("_MainTex");
        }

        // textureを設定
        block.SetTexture(shaderMainTexID, texture);

        // レンダラにPropertyBlockを設定
        GetComponent<Renderer>().SetPropertyBlock(block);
    }

    /// <summary>
    // メッシュ情報を更新
    /// </summary>
    public void UpdateMesh()
    {
        if (texture == null)
            return;

        // uvを計算
        var uvRect = new Rect(
            rect.x / texture.width, rect.y / texture.height,
            rect.width / texture.width, rect.height / texture.height);
        GetComponent<MeshFilter>().mesh = GetStoreMesh(uvRect);
    }

    /// <summary>
    // 指定のUV情報を持つメッシュを取得
    // 既にある場合はキャッシュから取得する
    /// </summary>
    /// <param name="uvRect">UV情報</param>
    /// <returns>指定のUV情報を持つメッシュ</returns>
    Mesh GetStoreMesh(Rect uvRect)
    {
        // UVが同じならば、再利用する
        var hashCode = uvRect.GetHashCode();
		if ( storedMesh.ContainsKey( hashCode ) && storedMesh[ hashCode ] != null ) {
			return storedMesh[ hashCode ];
		} else if ( quadMesh != null ) {
			// UVを設定したメッシュを生成
			var mesh = Instantiate( quadMesh );
			if ( storedMesh.ContainsKey( hashCode ) == false )
				storedMesh.Add( hashCode, mesh );
			else
				storedMesh[ hashCode ] = mesh;

			sharedUvs.Clear( );
			sharedUvs.Add( uvRect.min );
			sharedUvs.Add( uvRect.max );
			sharedUvs.Add( new Vector2( uvRect.x + uvRect.width, uvRect.y ) );
			sharedUvs.Add( new Vector2( uvRect.x, uvRect.y + uvRect.height ) );

			mesh.SetUVs( 0, sharedUvs );

			return mesh;
		} else {
			return new Mesh( );
		}
    }

    /// <summary>
    // 保持してるメッシュを全部捨てる
    /// </summary>
    public static void ClearStoredMesh()
    {
        foreach(var mesh in storedMesh)
        {
            Destroy(mesh.Value);
        }
        storedMesh.Clear();
    }
#endregion

#region Sprite Order

    [SerializeField]
    int sortingOrder;
    public int OrderInLayer {
        set
        {
            sortingOrder = value;
            UpdateSortingSetting();
        }
        get
        {
            return sortingOrder;
        }
    }

    [SerializeField, SortingLayerAttribute]
    string sortingLayerName;
    public string SortingLayerName{
        set
        {
            sortingLayerName = value;
            UpdateSortingSetting();
        }
        get
        {
            return sortingLayerName;
        }
    }

    /// <summary>
    /// ソート情報を更新する
    /// </summary>
    public void UpdateSortingSetting()
    {
        var renderer = GetComponent<Renderer>();
        renderer.sortingOrder = sortingOrder;
        renderer.sortingLayerName = sortingLayerName;
    }
#endregion

#region Unity Callback

    void OnValidate()
    {
        UpdateSortingSetting();
        UpdateTexture();
        UpdateMesh();
    }

    void Start ()
    {
        UpdateSortingSetting();
        UpdateTexture();
        UpdateMesh();
    }

    #endregion

#region Inspector Actions

    /// <summary>
    ///  テクスチャに合わせてRectを調整する
    /// </summary>
    [ContextMenu("Set Rect for Texture size")]
    void SetRectForTexture()
    {
        rect = new Rect(
            0, 0, 
            texture.width, texture.height
        );
        UpdateMesh();
    }

    [ContextMenu("Set Renderer Settings")]
    void SetRendererSetttings()
    {
        var renderer = GetComponent<Renderer>();
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
    }

    [ContextMenu("Fit Quad aspect by sprite")]
    void SetQuadAspect()
    {
        float xAspect = rect.width / rect.height;

        transform.localScale = new Vector3(transform.localScale.y * xAspect, transform.localScale.y, transform.localScale.z);
    }

#endregion
}
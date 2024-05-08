using UnityEngine;

public class able : MonoBehaviour
{
    public float targetAlpha = 80f; // 목표 투명도 값

    public LayerMask raycastMask; // 레이캐스트할 레이어 마스크
    public float maxRaycastDistance = 30f; // 레이캐스트의 최대 거리
    private Transform player; // 플레이어의 위치를 저장할 변수
    private Renderer[] lastHitRenderers; // 이전에 충돌한 오브젝트의 렌더러 배열

    void Start()
    {
        targetAlpha = 110f;
        maxRaycastDistance = 30f;

        // "Player" 태그가 설정된 오브젝트를 찾아 플레이어 변수에 할당합니다.
        player = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // 카메라에서 플레이어 위치로 레이를 쏩니다.
        Ray ray = new Ray(transform.position, player.position - transform.position);

        RaycastHit hit;

        // 이전에 충돌한 렌더러가 있다면 Opaque로 변경합니다.
        if (lastHitRenderers != null)
        {
            foreach (Renderer renderer in lastHitRenderers)
            {
                SetRendererOpaque(renderer);
            }
            lastHitRenderers = null;
        }

        // 레이캐스트가 성공했는지 확인합니다.
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, raycastMask))
        {
            // 플레이어와의 충돌이 아닌 경우에만 실행됩니다.
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("플레이어 이외의 오브젝트를 감지했습니다.");
                Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();
                lastHitRenderers = renderers;
                foreach (Renderer renderer in renderers)
                {
                    if (renderer != null && renderer.material != null)
                    {
                        // Rendering Mode를 Transparent로 변경
                        renderer.material.SetFloat("_Mode", 3); // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparent
                        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        renderer.material.SetInt("_ZWrite", 0);
                        renderer.material.DisableKeyword("_ALPHATEST_ON");
                        renderer.material.EnableKeyword("_ALPHABLEND_ON");
                        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;


                        // 시작 시 투명도를 110으로 설정
                        if (renderer.material.shader.name == "Standard (Specular setup)")
                        {
                            Color color = renderer.material.color;
                            color.a = targetAlpha / 255f;
                            renderer.material.color = color;
                        }
                        else if (renderer.material.shader.name == "Standard")
                        {
                            Color color = renderer.material.GetColor("_Color");
                            color.a = targetAlpha / 255f;
                            renderer.material.color = color;
                        }

                        
                    }
                }
                // 레이를 시각적으로 표시합니다.
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            }
        }
        else
        {
            // 플레이어와 충돌하지 않은 경우, 모든 렌더러를 Opaque로 변경합니다.
            if (lastHitRenderers != null)
            {
                foreach (Renderer renderer in lastHitRenderers)
                {
                    SetRendererOpaque(renderer);
                }
                lastHitRenderers = null;
            }
            // 레이를 시각적으로 표시합니다.
            Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.green);
        }
    }

    // 렌더러를 Opaque로 변경하는 함수
    void SetRendererOpaque(Renderer renderer)
    {
        if (renderer != null && renderer.material != null)
        {
            // Rendering Mode를 Opaque로 변경
            renderer.material.SetFloat("_Mode", 0); // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparent
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            renderer.material.SetInt("_ZWrite", 1);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.DisableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = -1; // Default


            if (renderer.material.shader.name == "Standard (Specular setup)")
            {
                Color color = renderer.material.color;
                color.a = 255f;
                renderer.material.color = color;
            }
            else if (renderer.material.shader.name == "Standard")
            {
                Color color = renderer.material.GetColor("_Color");
                color.a = 255;
                renderer.material.color = color;
            }            
        }
    }
}
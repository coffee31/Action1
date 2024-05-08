using UnityEngine;

public class able : MonoBehaviour
{
    public float targetAlpha = 80f; // ��ǥ ���� ��

    public LayerMask raycastMask; // ����ĳ��Ʈ�� ���̾� ����ũ
    public float maxRaycastDistance = 30f; // ����ĳ��Ʈ�� �ִ� �Ÿ�
    private Transform player; // �÷��̾��� ��ġ�� ������ ����
    private Renderer[] lastHitRenderers; // ������ �浹�� ������Ʈ�� ������ �迭

    void Start()
    {
        targetAlpha = 110f;
        maxRaycastDistance = 30f;

        // "Player" �±װ� ������ ������Ʈ�� ã�� �÷��̾� ������ �Ҵ��մϴ�.
        player = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // ī�޶󿡼� �÷��̾� ��ġ�� ���̸� ���ϴ�.
        Ray ray = new Ray(transform.position, player.position - transform.position);

        RaycastHit hit;

        // ������ �浹�� �������� �ִٸ� Opaque�� �����մϴ�.
        if (lastHitRenderers != null)
        {
            foreach (Renderer renderer in lastHitRenderers)
            {
                SetRendererOpaque(renderer);
            }
            lastHitRenderers = null;
        }

        // ����ĳ��Ʈ�� �����ߴ��� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, raycastMask))
        {
            // �÷��̾���� �浹�� �ƴ� ��쿡�� ����˴ϴ�.
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.Log("�÷��̾� �̿��� ������Ʈ�� �����߽��ϴ�.");
                Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();
                lastHitRenderers = renderers;
                foreach (Renderer renderer in renderers)
                {
                    if (renderer != null && renderer.material != null)
                    {
                        // Rendering Mode�� Transparent�� ����
                        renderer.material.SetFloat("_Mode", 3); // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparent
                        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        renderer.material.SetInt("_ZWrite", 0);
                        renderer.material.DisableKeyword("_ALPHATEST_ON");
                        renderer.material.EnableKeyword("_ALPHABLEND_ON");
                        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;


                        // ���� �� ������ 110���� ����
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
                // ���̸� �ð������� ǥ���մϴ�.
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            }
        }
        else
        {
            // �÷��̾�� �浹���� ���� ���, ��� �������� Opaque�� �����մϴ�.
            if (lastHitRenderers != null)
            {
                foreach (Renderer renderer in lastHitRenderers)
                {
                    SetRendererOpaque(renderer);
                }
                lastHitRenderers = null;
            }
            // ���̸� �ð������� ǥ���մϴ�.
            Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance, Color.green);
        }
    }

    // �������� Opaque�� �����ϴ� �Լ�
    void SetRendererOpaque(Renderer renderer)
    {
        if (renderer != null && renderer.material != null)
        {
            // Rendering Mode�� Opaque�� ����
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
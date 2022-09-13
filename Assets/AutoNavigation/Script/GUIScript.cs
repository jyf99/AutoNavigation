using UnityEngine;
using UnityEngine.AI;

public class GUIScript : MonoBehaviour
{
    public GUISkin guiSkin;
    private bool isShowing = false;
    private Rect windowRect = new Rect(Screen.width * 0.2f, Screen.height * 0.1f, Screen.width * 0.6f, Screen.height * 0.8f);

    private NavMeshAgent agent;
    private Vector3 positon;
    private Vector3 velocity;
    private float distance = 0f;
    private float time = 0f;
    private float speedScore = 85;
    private float accelerationScore = 95;
    private long showTipTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        positon = agent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnGUI()
    {
        GUI.skin = guiSkin;

        if (GUI.Button(new Rect(50, 50, 150, 50), "Driving Board"))
        {
            isShowing = !isShowing;
        }

        if (null == agent)
        {
            return;
        }

        if (isShowing)
        {

            GUI.Window(0, windowRect, showDrivingBoard, "Driving Board");
        }

        if (null != velocity && Vector3.Angle(agent.velocity, velocity) > 0.3 && showTipTime == 0)
        {
            showTipTime = (int)time;
        }
        velocity = agent.velocity;
        if (showTipTime != 0 && time - showTipTime < 5)
        {
            showDrivingTips("Cornering");
        } else
        {
            showTipTime = 0;
        }
        if (time < 5)
        {
            showDrivingTips("Acceleration");
        }
    }

    private void showDrivingBoard(int windowId)
    {
        float startY = windowRect.height * 0.1f;
        float boxW = windowRect.width * 0.15f;
        float boxH = windowRect.height * 0.1f;
        float startX = (windowRect.width - 3 * boxW) / 4;
        float divider = startX + boxW;
        float speed = (int)(agent.velocity.magnitude * 3600 / 1000f * 100) / 100f;
        GUI.Label(new Rect(startX, startY, boxW, boxH), "Speed: " + speed + "Km/h");
        distance += (agent.transform.position - positon).magnitude;
        positon = agent.transform.position;
        float tmpDistance = (int)(distance / 1000 * 100) / 100f;
        GUI.Label(new Rect(startX + divider, startY, boxW, boxH), "Distance: " + tmpDistance + "Km");
        float tmpTime = (int)(time / 3600 * 100) / 100f;
        GUI.Label(new Rect(startX + divider * 2, startY, boxW, boxH), "Duration: " + tmpTime + "h");

        float boxY = windowRect.height * 0.25f;
        Rect boxRect = new Rect(windowRect.width * 0.1f, boxY, windowRect.width * 0.8f, windowRect.height * 0.8f);
        GUI.Box(boxRect, $"Total Score: {(int) (speedScore + accelerationScore + 100) / 3}");
        float minScore = 0f;
        float maxScore = 100f;
        float labelW = boxRect.width * 0.15f;
        float labelH = boxRect.height * 0.05f;
        float labelX = windowRect.width * 0.15f;
        float dividerH = (boxRect.height - 3 * labelH) / 5;
        float labelY = boxY + dividerH;
        float sliderW = boxRect.width * 0.7f;
        float sliderH = boxRect.height * 0.05f;
        float sliderX = labelX + labelW + labelW * 0.05f;
        float sliderY = labelY + labelH - sliderH;
        float labelDivider = dividerH + labelH;
        float sliderDivider = dividerH + sliderH;
        if ((int) (time % 15) == 0)
        {
            speedScore = Random.Range(80, 100);
            accelerationScore = Random.Range(80, 100);
        }
        GUI.Label(new Rect(labelX, labelY, labelW, labelH), $"Speed[{speedScore}]");
        GUI.HorizontalSlider(new Rect(sliderX, sliderY, sliderW, sliderH), speedScore, minScore, maxScore);
        GUI.Label(new Rect(labelX, labelY + labelDivider, labelW, labelH), "Weather[100]");
        GUI.HorizontalSlider(new Rect(sliderX, sliderY + sliderDivider, sliderW, sliderH), 100f, minScore, maxScore);
        GUI.Label(new Rect(labelX, labelY + labelDivider * 2, labelW, labelH), $"Acceleration[{accelerationScore}]");
        GUI.HorizontalSlider(new Rect(sliderX, sliderY + sliderDivider * 2, sliderW, sliderH), accelerationScore, minScore, maxScore);
    }

    private void showDrivingTips(string msg)
    {
        GUI.Label(new Rect(Screen.width * 0.85f, Screen.height * 0.1f, Screen.width * 0.1f, Screen.height * 0.05f), msg);
    }
}

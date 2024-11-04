using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Serialization;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TaskStation : MonoBehaviour
{
    public TaskStationInfo TaskStationInfo;
    public Transform EntityTarget;
    private QueueManager queueManager;
    [SerializeField, FormerName("queuePos")] List<Transform> _queuePos;
    [SerializeField, FormerName("taskPos")] Transform _taskPos;

    [TitleGroup("UI")]
    [SerializeField] Transform infoPanel;
    [SerializeField] TextMeshProUGUI stationNameTxt, taskDurationTxt;
    [SerializeField] Button upgradeBtn;
    Tween panelTween;
    bool isOpen;

    private bool isBusy = false;
    private int level;
    private void Start()
    {
        queueManager = new QueueManager(_queuePos);
        stationNameTxt.text = TaskStationInfo.StationName;
        taskDurationTxt.text = $"{TaskStationInfo.BaseDuration.ToString()}s";
        upgradeBtn.onClick.AddListener(() =>
        {
            Debug.Log("Upgrade");
        });
    }

    public void AssignEmployeeToQueue(Employee _employee)
    {
        queueManager.AddToQueue(_employee);
        if (!isBusy)
        {
            StartTask(_employee);
        }
    }

    private async void StartTask(Employee _employee)
    {
        await UniTask.WaitUntil(() => !isBusy);
        isBusy = true;
        Pet _currentPet = _employee.currentTask.pet;

        // Move employee to station
        await _employee.SetTarget(EntityTarget);

        // Once arrived, request service
        _employee.DropOffPet();
        _currentPet.transform.parent = _taskPos;
        _currentPet.transform.localPosition = Vector3.zero;
        _employee.ShowProgress(TaskStationInfo.BaseDuration);
        await UniTask.WaitForSeconds(TaskStationInfo.BaseDuration);
        _employee.PickupPet(_employee.currentTask.pet);
        _employee.NextTask();



        // Process next in queue
        queueManager.RemoveFromQueue();
        ProcessNextInQueue();

        isBusy = false;
        // Mark the station as free after task completion

    }

    public void ProcessNextInQueue()
    {
        if (queueManager.HasWaitingEntities())
        {
            Employee nextEmployee = (Employee)queueManager.GetNextInQueue();
            StartTask(nextEmployee);
        }
    }

    //UI
    private void OnMouseUpAsButton()
    {
        ToggleInfoMenu();
    }

    void ToggleInfoMenu()
    {
        if (isOpen)
        {
            isOpen = false;
            infoPanel.localScale = Vector3.one;
            panelTween?.Kill();
            panelTween = infoPanel.DOScale(0, .2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                infoPanel.gameObject.SetActive(false);
            });
        }
        else
        {
            isOpen = true;
            infoPanel.localScale = Vector3.zero;
            infoPanel.gameObject.SetActive(true);
            panelTween?.Kill();
            panelTween = infoPanel.DOScale(1, .2f).SetEase(Ease.OutBack);
        }
    }

}

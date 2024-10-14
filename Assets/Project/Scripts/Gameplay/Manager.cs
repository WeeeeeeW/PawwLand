using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Manager : Entity
{
    [SerializeField][ReadOnly] Counter counter;
    private TaskManager taskManager;
    public bool isAvailable;
    public float efficiency = 1;
    private void Awake()
    {
        navMeshAgent = GetComponent<FollowerEntity>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        counter.isAvailable = true;
    }
    public void AssignToCounter(Counter _counter)
    {
        counter = _counter;
    }

    public async void AssignTask(ServiceType _service, Pet _pet)
    {
        counter.isAvailable = false;
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
        await SetTarget(taskManager.petzone.dropoff);
        await UniTask.WaitForSeconds(taskManager.petzone.taskDuration);
        DropPetIntoPetZone(_service, _pet);
    }
    public async void DropPetIntoPetZone(ServiceType _service, Pet _pet)
    {
        taskManager.AssignPetToZone(_pet);
        taskManager.CreateTask(_pet.owner, _pet, _service);
        await SetTarget(taskManager.employeeCounter);
        counter.isAvailable = true;
        transform.DORotateQuaternion(counter.transform.rotation,.2f);
    }
    IEnumerator CorrectRotationToCounter()
    {
        yield return new WaitForSeconds(.1f);
        Vector3 startRotation = transform.forward;
        float rotationSpeed = 600f;
        Vector3 desiredRotation = counter.transform.position - transform.position;
        desiredRotation.y = 0;
        float rotationAngle = Vector3.Angle(transform.forward, desiredRotation);
        float rotationDuration = rotationAngle / rotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.forward = Vector3.Lerp(startRotation, desiredRotation, elapsedTime / rotationDuration);
            yield return null;
        }
    }

}

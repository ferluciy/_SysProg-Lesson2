using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Zadanie2 : MonoBehaviour
{
    private JobHandle _jobHandle;
    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _finalPositions;

    public struct MyJob : IJobParallelFor
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> FinalPositions;
        public void Execute(int index)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
        }
    }
    void Start()
    {
        _positions = new NativeArray<Vector3>(20, Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(20, Allocator.Persistent);
        _finalPositions = new NativeArray<Vector3>(20, Allocator.Persistent);

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = new Vector3(0, 1, i);
            _velocities[i] = new Vector3(1, i, 0);
        }

        MyJob myJob = new MyJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            FinalPositions = _finalPositions
        };

        _jobHandle = myJob.Schedule(_positions.Length, 5);
        _jobHandle.Complete();

        StartCoroutine(JobCoroutine());
    }

    private IEnumerator JobCoroutine()
    {
        while (_jobHandle.IsCompleted == false)
        {
            yield return new WaitForEndOfFrame();
        }
        foreach (Vector3 vector in this._finalPositions)
        {
            print(vector);
        }
        _positions.Dispose();
        _velocities.Dispose();
        _finalPositions.Dispose();
    }
}

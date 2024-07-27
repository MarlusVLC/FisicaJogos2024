using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using _6.AcaoReacao;
using UnityEngine;

public abstract class Tracker<T> : Singleton<Tracker<T>> where T : Tracked<T>
{
    protected Dictionary<int, T> trackedCollection = new();
    private int keyCount;

    public IReadOnlyDictionary<int, T> TrackedCollection => trackedCollection;

    public int StartTracking(T newTrack)
    {
        if (newTrack.SystemID >= 0 && trackedCollection.ContainsKey(newTrack.SystemID) == false)
        {
            trackedCollection.Add(newTrack.SystemID, newTrack);
            return newTrack.SystemID;
        }
        trackedCollection.Add(keyCount, newTrack);
        return keyCount++;
    }
    
    public void StopTracking(Tracked<T> tracked)
    {
        if (tracked.SystemID < 0)
        {
            throw new InvalidOperationException($"{tracked.name} hasn't been added to the {Instance.name}!");
        }
        if (trackedCollection.ContainsKey(tracked.SystemID) == false)
        {
            throw new KeyNotFoundException($"{tracked.name} couldn't be found on the {Instance.name}!");
        }
        Debug.Log($"{tracked.name} has been REMOVED and it had the following SystemID = {tracked.SystemID}");
        trackedCollection.Remove(tracked.SystemID);
    }

}
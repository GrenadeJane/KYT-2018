using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Utils for coroutines.
/// </summary>

public static class CoroutineUtils
{
    #region Types


    // MonoBehaviour to use for the global object:
    class CoroutineUtilsBehaviour : MonoBehaviour
    {
        // Empty.
    }


    #endregion








    #region Runtime data


    // Global MonoBehaviour object to execute the coroutine methods:
    static CoroutineUtilsBehaviour _globalMonoBehaviour;

    static CoroutineUtilsBehaviour GlobalMonoBehaviour
    {
        get
        {
            if (_globalMonoBehaviour == null)
            {
                _globalMonoBehaviour = new GameObject("Coroutine utils object").AddComponent<CoroutineUtilsBehaviour>();
                _globalMonoBehaviour.hideFlags = HideFlags.HideAndDontSave;
                UnityEngine.Object.DontDestroyOnLoad(_globalMonoBehaviour);
            }

            return _globalMonoBehaviour;
        }
    }


    #endregion








    /// <summary>
    /// Starts a coroutine using the specified <see cref="MonoBehaviour"/>, or a global one if <c>null</c>.
    /// </summary>
    /// <param name="monoBehaviour">The <see cref="MonoBehaviour"/> to use, <c>null</c> to use a global one.</param>
    /// <param name="routine">The <see cref="IEnumerator"/> to use for the coroutine.</param>
    /// <returns>The started coroutine.</returns>

    static Coroutine StartCoroutine(MonoBehaviour monoBehaviour, IEnumerator routine)
    {
        return (monoBehaviour ?? GlobalMonoBehaviour).StartCoroutine(routine);
    }








    /// <summary>
    /// Starts a coroutine tied to a global and always active <see cref="MonoBehaviour"/>.
    /// </summary>
    /// <param name="routine">The <see cref="IEnumerator"/> to use for the coroutine.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine StartGlobalCoroutine(IEnumerator routine)
    {
        return GlobalMonoBehaviour.StartCoroutine(routine);
    }








    /// <summary>
    /// Stops a global coroutine started with <see cref="StartGlobalCoroutine(IEnumerator)"/>, or passing null as the MonoBehaviour to the methods that create coroutines.
    /// </summary>
    /// <param name="coroutine">The coroutine to stop.</param>

    public static void StopGlobalCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
            GlobalMonoBehaviour.StopCoroutine(coroutine);
    }








    /// <summary>
    /// Stops a coroutine, ands sets the variable to null.
    /// </summary>
    /// <param name="monoBehaviour">The <see cref="MonoBehaviour"/> that created the coroutine, <c>null</c> if a global one was used.</param>
    /// <param name="coroutine">The coroutine to stop. There's no error if it's already null.</param>

    public static void StopCoroutineAndSetToNull(MonoBehaviour monoBehaviour, ref Coroutine coroutine)
    {
        if (coroutine == null)
            return;

        if (monoBehaviour == null)
            StopGlobalCoroutine(coroutine);
        else
            monoBehaviour.StopCoroutine(coroutine);

        coroutine = null;
    }








    #region ExecuteWhenFinished() methods


    /// <summary>
    /// Starts a coroutine that executes the specified method when a "waitable operation" finishes its execution.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="waitableOperation">The action to wait for. Can be null, in which case the method will be executed immediately.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The started coroutine, that will end just after executing the action, or null if <paramref name="waitableOperation"/> was null.</returns>

    public static Coroutine ExecuteWhenFinished(MonoBehaviour monoBehaviour, YieldInstruction waitableOperation, Action action)
    {
        if (waitableOperation == null)
        {
            action();
            return null;
        }
        else
        {
            return StartCoroutine(monoBehaviour, ExecuteWhenFinished_Coroutine(waitableOperation, action));
        }
    }








    static IEnumerator ExecuteWhenFinished_Coroutine(YieldInstruction waitableOperation, Action action)
    {
        yield return waitableOperation;
        action();
    }








    /// <summary>
    /// Starts a coroutine that executes the specified method when a "waitable operation" finishes its execution.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="waitableOperation">The action to wait for. Can be null, in which case the method will be executed immediately.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The started coroutine, that will end just after executing the action, or null if <paramref name="waitableOperation"/> was null.</returns>

    public static Coroutine ExecuteWhenFinished(MonoBehaviour monoBehaviour, CustomYieldInstruction waitableOperation, Action action)
    {
        if (waitableOperation == null || !waitableOperation.keepWaiting)
        {
            action();
            return null;
        }
        else
        {
            return StartCoroutine(monoBehaviour, ExecuteWhenFinished_Coroutine(waitableOperation, action));
        }
    }








    static IEnumerator ExecuteWhenFinished_Coroutine(CustomYieldInstruction waitableOperation, Action action)
    {
        yield return waitableOperation;
        action();
    }








    /// <summary>
    /// Starts a coroutine that executes the specified method when a "waitable operation" finishes its execution.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="waitableOperation">The action to wait for. Can be null, in which case the method will be executed immediately.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The started coroutine, that will end just after executing the action, or null if <paramref name="waitableOperation"/> was null.</returns>

    public static Coroutine ExecuteWhenFinished(MonoBehaviour monoBehaviour, IEnumerator waitableOperation, Action action)
    {
        if (waitableOperation == null)
        {
            action();
            return null;
        }
        else
        {
            return StartCoroutine(monoBehaviour, ExecuteWhenFinished_Coroutine(waitableOperation, action));
        }
    }








    static IEnumerator ExecuteWhenFinished_Coroutine(IEnumerator waitableOperation, Action action)
    {
        yield return waitableOperation;
        action();
    }


    #endregion








    #region WaitFor() methods


    /// <summary>
    /// Starts a coroutine that ends when the specified operation ends.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation">The operation to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitFor(MonoBehaviour monoBehaviour, YieldInstruction operation)
    {
        if (operation == null)
            return null;

        return StartCoroutine(monoBehaviour, WaitFor_Coroutine(monoBehaviour, operation));
    }








    static IEnumerator WaitFor_Coroutine(MonoBehaviour monoBehaviour, YieldInstruction operation)
    {
        yield return operation;
    }








    /// <summary>
    /// Starts a coroutine that ends when the specified operation ends.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation">The operation to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitFor(MonoBehaviour monoBehaviour, CustomYieldInstruction operation)
    {
        if (operation == null || !operation.keepWaiting)
            return null;

        return StartCoroutine(monoBehaviour, WaitFor_Coroutine(monoBehaviour, operation));
    }








    static IEnumerator WaitFor_Coroutine(MonoBehaviour monoBehaviour, CustomYieldInstruction operation)
    {
        yield return operation;
    }


    #endregion








    #region WaitForAll() methods


    /// <summary>
    /// Starts a coroutine that ends when all the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation0">One of the operations to wait for.</param>
    /// <param name="operation1">One of the operations to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, YieldInstruction operation0, YieldInstruction operation1)
    {
        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operation0, operation1, null));
    }








    /// <summary>
    /// Starts a coroutine that ends when all the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation0">One of the operations to wait for.</param>
    /// <param name="operation1">One of the operations to wait for.</param>
    /// <param name="operation2">One of the operations to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, YieldInstruction operation0, YieldInstruction operation1, YieldInstruction operation2)
    {
        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operation0, operation1, operation2));
    }








    static IEnumerator WaitForAll_Coroutine(MonoBehaviour monoBehaviour, YieldInstruction operation0, YieldInstruction operation1, YieldInstruction operation2)
    {
        if (operation0 != null) yield return operation0;
        if (operation1 != null) yield return operation1;
        if (operation2 != null) yield return operation2;
    }








    /// <summary>
    /// Starts a coroutine that ends when all the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation0">One of the operations to wait for.</param>
    /// <param name="operation1">One of the operations to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, CustomYieldInstruction operation0, CustomYieldInstruction operation1)
    {
        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operation0, operation1, null));
    }








    /// <summary>
    /// Starts a coroutine that ends when all the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operation0">One of the operations to wait for.</param>
    /// <param name="operation1">One of the operations to wait for.</param>
    /// <param name="operation2">One of the operations to wait for.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, CustomYieldInstruction operation0, CustomYieldInstruction operation1, CustomYieldInstruction operation2)
    {
        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operation0, operation1, operation2));
    }








    static IEnumerator WaitForAll_Coroutine(MonoBehaviour monoBehaviour, CustomYieldInstruction operation0, CustomYieldInstruction operation1, CustomYieldInstruction operation2)
    {
        if (operation0 != null && operation0.keepWaiting) yield return operation0;
        if (operation1 != null && operation1.keepWaiting) yield return operation1;
        if (operation2 != null && operation2.keepWaiting) yield return operation2;
    }








    /// <summary>
    /// Starts a coroutine that ends when all of the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operations">The operations to wait for.</param>
    /// <returns>The started coroutine, or null if <paramref name="operations"/> is null.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, params YieldInstruction[] operations)
    {
        if (operations == null)
            return null;

        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operations));
    }








    static IEnumerator WaitForAll_Coroutine(MonoBehaviour monoBehaviour, YieldInstruction[] operations)
    {
        for (int i = 0; i < operations.Length; i++)
        {
            YieldInstruction operation = operations[i];

            if (operation != null)
                yield return operation;
        }
    }








    /// <summary>
    /// Starts a coroutine that ends when all of the specified operations have ended.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="operations">The operations to wait for.</param>
    /// <returns>The started coroutine, or null if <paramref name="operations"/> is null.</returns>

    public static Coroutine WaitForAll(MonoBehaviour monoBehaviour, params CustomYieldInstruction[] operations)
    {
        if (operations == null)
            return null;

        return StartCoroutine(monoBehaviour, WaitForAll_Coroutine(monoBehaviour, operations));
    }








    static IEnumerator WaitForAll_Coroutine(MonoBehaviour monoBehaviour, CustomYieldInstruction[] operations)
    {
        for (int i = 0; i < operations.Length; i++)
        {
            CustomYieldInstruction operation = operations[i];

            if (operation != null && operation.keepWaiting)
                yield return operation;
        }
    }


    #endregion








    /// <summary>
    /// Starts a coroutine that executes the specified action after a number of frames have passed.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="frames">The number of frames to wait; for example, 1 means that the action must be executed on the next frame. Must be > 0.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine ExecuteAfterFrames(MonoBehaviour monoBehaviour, int frames, Action action)
    {
        return StartCoroutine(monoBehaviour, ExecuteAfterFrames_Coroutine(frames, action));
    }








    static IEnumerator ExecuteAfterFrames_Coroutine(int frames, Action action)
    {
        while (frames-- > 0)
            yield return null;

        action();
    }








    #region Looping methods


    /// <summary>
    /// Starts a coroutine that performs some actions in a loop.
    /// It's guaranteed to end the loop with a value of 1.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="duration">The duration of the animation.</param>
    /// <param name="loopStepAction">The code to execute on each loop step. Will receive the normalized time.</param>
    /// <param name="finishedAction">The action to execute when the loop finishes. Can be null.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine StartLoop(MonoBehaviour monoBehaviour, float duration, Action<float> loopStepAction, Action finishedAction = null)
    {
        if (duration > 0)
        {
            return StartCoroutine(monoBehaviour, StartLoop_Coroutine(duration, loopStepAction, finishedAction));
        }
        else
        {
            loopStepAction(1);
            if (finishedAction != null)
                finishedAction();
            return null;
        }
    }








    static IEnumerator StartLoop_Coroutine(float duration, Action<float> loopStepAction, Action finishedAction)
    {
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;
            float normTime = Mathf.Min(1.0f, elapsed / duration);
            loopStepAction(normTime);

            if (normTime == 1f)
                break;
            else
                yield return null;
        }

        if (finishedAction != null)
            finishedAction();
    }








    /// <summary>
    /// Starts a coroutine that performs some actions in a loop, and which can be cancelled at any moment.
    /// It's guaranteed to reach the final value of 1.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="duration">The duration of the animation.</param>
    /// <param name="loopStepAction">The code to execute on each loop step. Will receive the normalized time, and must return true to continue the loop or false to stop it at any time.</param>
    /// <param name="finishedAction">The action to execute when the loop finishes, either normally or by manually stopping it returning false from the step action method. Can be null.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine StartCancelableLoop(MonoBehaviour monoBehaviour, float duration, Func<float, bool> loopStepAction, Action finishedAction = null)
    {
        if (duration > 0)
        {
            return StartCoroutine(monoBehaviour, StartCancelableLoop_Coroutine(duration, loopStepAction, finishedAction));
        }
        else
        {
            loopStepAction(1);
            if (finishedAction != null)
                finishedAction();
            return null;
        }
    }








    static IEnumerator StartCancelableLoop_Coroutine(float duration, Func<float, bool> loopStepAction, Action finishedAction)
    {
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;
            float normTime = Mathf.Min(1.0f, elapsed / duration);
            bool mustContinue = loopStepAction(normTime);

            if (!mustContinue || normTime == 1f)
                break;
            else
                yield return null;
        }

        if (finishedAction != null)
            finishedAction();
    }








    /// <summary>
    /// Starts a coroutine that loops until canceled, executing an action each frame.
    /// </summary>
    /// <param name="monoBehaviour">The <c>MonoBehaviour</c> to use to run the coroutine. <c>null</c> to use a global default one, which is always active.</param>
    /// <param name="loopStepAction">The code to execute on each loop step. Must return true to continue the loop or false to stop it at any time. It will be called immediately when the method executes.</param>
    /// <returns>The started coroutine.</returns>

    public static Coroutine StartCancelableLoop(MonoBehaviour monoBehaviour, Func<bool> loopStepAction)
    {
        return StartCoroutine(monoBehaviour, StartCancelableLoop_Coroutine(loopStepAction));
    }








    static IEnumerator StartCancelableLoop_Coroutine(Func<bool> loopStepAction)
    {
        while (loopStepAction())
        {
            yield return null;
        }
    }


    #endregion
}
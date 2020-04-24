using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour
{
	private List<ICommand> commandBuffer = new List<ICommand>();

	public void AddCommand(ICommand command)
	{
		commandBuffer.Add(command);
	}

	public void Rewind()
	{
		StartCoroutine(RewindRoutine());
	}

	IEnumerator RewindRoutine()
	{
		Debug.Log("Rewinding...");
		foreach (ICommand command in Enumerable.Reverse(commandBuffer))
		{
			command.Undo();
			yield return new WaitForEndOfFrame();
		}
	}

	public void Play()
	{
		StartCoroutine(PlayRoutine());
	}

	IEnumerator PlayRoutine()
	{
		Debug.Log("Playing...");
		foreach (ICommand command in commandBuffer)
		{
			command.Execute();
			yield return new WaitForEndOfFrame();
		}
	}


}

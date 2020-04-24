using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : ICommand
{
	private Transform playerPos;
	private float playerSpeed;


	public MoveLeftCommand(Transform player, float speed)
	{
		this.playerPos = player;
		this.playerSpeed = speed;
	}

	public void Execute()
	{
		playerPos.Translate(Vector3.left * playerSpeed * Time.deltaTime);
	}
	
	public void Undo()
	{
		playerPos.Translate(Vector3.right * playerSpeed * Time.deltaTime);
	}
}

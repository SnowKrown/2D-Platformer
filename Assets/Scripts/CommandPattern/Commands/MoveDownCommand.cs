using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownCommand : ICommand
{
	private Transform playerPos;
	private float playerSpeed;


	public MoveDownCommand(Transform player, float speed)
	{
		this.playerPos = player;
		this.playerSpeed = speed;
	}

	public void Execute()
	{
		playerPos.Translate(Vector3.down * playerSpeed * Time.deltaTime);
	}
	public void Undo()
	{
		playerPos.Translate(Vector3.up * playerSpeed * Time.deltaTime);
	}
}

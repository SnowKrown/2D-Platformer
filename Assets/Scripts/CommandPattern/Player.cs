using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private ICommand moveUp, moveLeft, moveRight, moveDown;
	private CommandManager commandManager;

	[SerializeField] private float speed = 1;
    
	private void Awake()
    {
		commandManager = FindObjectOfType<CommandManager>();
    }

    private void Update()
    {
		if (Input.GetKey(KeyCode.W))
		{
			moveUp = new MoveUpCommand(this.transform,speed);
			moveUp.Execute();
			commandManager.AddCommand(moveUp);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			moveDown = new MoveDownCommand(this.transform, speed);
			moveDown.Execute();
			commandManager.AddCommand(moveDown);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			moveLeft = new MoveLeftCommand(this.transform, speed);
			moveLeft.Execute();
			commandManager.AddCommand(moveLeft);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			moveRight = new MoveRightCommand(this.transform, speed);
			moveRight.Execute();
			commandManager.AddCommand(moveRight);
		}
	}
}

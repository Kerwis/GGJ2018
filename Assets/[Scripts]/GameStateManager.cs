using System;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager
{

	private static GameState _currentGameState ;

	public static Action<GameState> GameStateChange;

	public static void ChangeGameState(GameState state)
	{
		if(_currentGameState == state)
			return;
		
		_currentGameState = state;

		if (GameStateChange != null)
			GameStateChange(state);
	}

}


public enum DIRECTION {
    LEFT = 1,
    RIGHT = -1
}

public enum FADE_DIRECTION : ushort {
    IN = 1,
    OUT = 0
}

public enum GameState {
    MAIN_MENU,
    GAME_START,
    GAME_RUNNING,
    SETTINGS_MENU,
    GAME_OVER,
    NONE
}
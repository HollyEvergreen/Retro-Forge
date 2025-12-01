# initialise our engine
-   Create window
-   Load assets
# inject the game code
```c#
interface IGame{
    IGame Init();
    IGame MainLoop();
    float Render();
    float Update(float dt);
}

class Game : IGame{
    Game Init(){
        ...
    }
    Game MainLoop(){
        ...
    }
    float Render(){
        ...
    }
    float Update(float dt){
        ...
    }
}

void Main(string[] args){
    var game = Game();
    Game.Init()
        .MainLoop();
}
```
# run the game
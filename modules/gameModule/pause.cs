if (!isObject(PauseBahavior))  
{  
   %template = new BehaviorTemplate(PauseBahavior);  
     
   %template.friendlyName = "Pause Game";  
   %template.behaviorType = "Scene";  
   %template.description  = "Pause and unpause the game";  
  
   %template.addBehaviorField(pauseKey, "The button to pause the game", Keybind, "keyboard P");  
}  
  
function PauseBahavior::onBehaviorAdd(%this)  
{  
   if (!isObject(moveMap))  
      return;  
        
   moveMap.bindCmd( getWord(%this.pauseKey, 0), getWord(%this.pauseKey, 1), "pauseGame();", "");  
  
   $isPaused = false;  
}  
  
function pauseGame()  
{  
   if($isPaused)  
   {     
      $timescale=1;       
   }  
   else  
   {  
      $timescale=0;  
   }  
     
   $isPaused = !$isPaused;  
}  
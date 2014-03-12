function GameModule::create( %this ) {
	exec( "./gui/guiProfiles.cs" );
	exec( "./scripts/gameWindow.cs" );
	exec( "./scripts/gameScene.cs" );
	exec( "./scripts/genWorld.cs" );
	exec( "./scripts/background.cs" );
	exec( "./scripts/controls.cs" );

	createSceneWindow();
	createScene();
	Window.setScene( GameScene );
	GameScene.setDebugOn( "collision", "position", "aabb" );
	
	createWorld();
	
	createBackground();

	//new ScriptObject( InputManager );
	//window.addInputListener( InputManager );
	//InputManager.init();
	
	//createEnemy();
	//createItem();
}

function GameModule::destroy( %this ) {
	destroySceneWindow();
	//controls.pop();
}

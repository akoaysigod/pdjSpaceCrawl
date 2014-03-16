function createScene() {
	exec( "./genWorld.cs" );
	if ( isObject( GameScene ) ) {
			destroyScene();
	}

	if ( isObject( Ship ) ) {
		Ship.delete();
	}

	if ( isObject( MainMenuScene ) ) {
		for ( %i = 0; %i != MainMenuScene.getCount(); %i++ ) {
			%t = MainMenuScene.getObject( %i );
			%t.setUseInputEvents( false );
			%t.safeDelete();
		}
		//???
		MainMenuScene.clear( false );
	}
	
	Window.setUseObjectInputEvents( false );
	new Scene( GameScene );
	Window.setScene( GameScene );
	createWorld();
}

function destroyScene() { 
		if ( !isObject( GameScene ) ) {
				return; 
		}
		
		scene.delete();
}

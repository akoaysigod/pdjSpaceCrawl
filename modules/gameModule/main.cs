function GameModule::create( %this ) {
	exec( "./gui/guiProfiles.cs" );
	exec( "./scripts/gameWindow.cs" );
	exec( "./scripts/gameScene.cs" );
	exec( "./scripts/genWorld.cs" );
	exec( "./scripts/controls.cs" );

	createSceneWindow();
	createScene();
	Window.setScene( GameScene );
	//GameScene.setDebugOn( "collision", "position", "aabb" );
	
	GameScene.playWidth = 200;
	GameScene.playHeight = 100; 
	GameScene.factor = 2;
	createWorld();
	
	setRandomSeed( getRealTime() );  
	//new ScriptObject( InputManager );
	//window.addInputListener( InputManager );
	//InputManager.init();
	
	//createEnemy();
	//createItem();
}

function GameModule::destroy( %this ) {
	destroySceneWindow();
}

function pause() {
	%k = GameScene.getCount();
	for ( %i = 0; %i != %k; %i++ ) {
		%tmp = GameScene.getObject( %i );

		if ( %tmp.name $= "block" ) {
			continue;
		}

		%tmp.pauseVel = %tmp.getLinearVelocity();
		%tmp.setLinearVelocity( "0 0" );
		%tmp.pauseAng = %tmp.getAngularVelocity();
		%tmp.setAngularVelocity( 0 );

		if ( !%tmp.isMoveToComplete() ) {
			%tmp.hadMoveTo = true;
			//echo( %tmp.name SPC %tmp.speed SPC %tmp.movePos );
		} else { 
			%tmp.hadMoveTo = false; 
		}

		if ( %tmp.isTimerActive() ) {
			%tmp.hasTimer = true;
			%tmp.stopTimer();
		} else { 
			%tmp.hasTimer = false;
		}

		if ( %tmp.getUpdateCallback() ) {
			%tmp.isUpdating = true;
			%tmp.setUpdateCallback( false );
		} else {
			%tmp.isUpdating = false;
		}
	}
	$pauseStatus = true;
}

function unpause() {
	if ( !isObject( GameScene ) ) {
		return;
	}

	%k = GameScene.getCount();
	for ( %i = 0; %i != %k; %i++ ) {
		%tmp = GameScene.getObject( %i );

		if ( %tmp.name $= "block" ) {
			continue;
		}

		%tmp.setLinearVelocity( %tmp.pauseVel );
		%tmp.setAngularVelocity( %tmp.pauseAng );

		if ( %tmp.hadMoveTo ) {
			%tmp.moveTo( %tmp.movePos, %tmp.speed, false, false );
		}

		if ( %tmp.hasTimer ) {
			%tmp.reactivateTimer();
		} 

		if ( %tmp.isUpdating ) {
			%tmp.setUpdateCallback( true );
		} 

	}	
	Ship.setAngularVelocity( 0 );
	$pauseStatus = false;
}
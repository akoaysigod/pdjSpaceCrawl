function GameModule::create( %this ) {
	exec( "./gui/guiProfiles.cs" );
	exec( "./scripts/gameWindow.cs" );
	exec( "./scripts/gameScene.cs" );
	exec( "./scripts/genWorld.cs" );
	exec( "./scripts/controls.cs" );
	exec( "./scripts/mainMenu.cs" );

	createSceneWindow();
	createMainMenu();

	setRandomSeed( getRealTime() );  
	
	//GameScene.setDebugOn( "collision", "position", "aabb" );
}

function GameModule::destroy( %this ) {
	alxStopAll();
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

function gameOver() {
	pause();
	Ship.hasGameOver = true;
	saveGame();

	for ( %i = 0; %i != GameScene.getCount(); %i ++ ) {
		%t = gameScene.getObject( %i );
		%t.safeDelete();
	}
	
	for ( %i = 0; %i != UIScene.getCount(); %i++ ) {
		%t = UIScene.getObject( %i );
		%t.setVisible( false );
	}

	%fin = new ImageFont() {
		image = "gameModule:font";
		Text = "GAME OVER";
		FontSize = "10";
		Position = "0 10";
		SceneLayer = "0";
		BlendColor = "255, 0, 0";
		TextAlignment = "center";
		LifeTime = 10;
	};
	UIScene.add( %fin );

	%cont = new ImageFont() {
		image = "gameModule:font";
		Text = "Press escape to continue.";
		FontSize = "3";
		Position = "0 0";
		SceneLayer = "0";
		BlendColor = "255, 0, 0";
		TextAlignment = "center";
		LifeTime = 10;
	};
	UIScene.add( %cont );

	%input = new ScriptObject( GameOverInput );
	Window.addInputListener( %input );
	%controls = new ActionMap( GameOverControls );
	%controls.bindCmd( keyboard, "escape", "nothing();", "createMainMenu();" );
	%controls.push();
}

function saveGame() {
	Ship.planetID = Window.planetID;
	if ( Window.planetID == 0 ) {
		Ship.planetID += 1;
	}
	
	TamlWrite( Mothership, "modules/saveFiles/mothership.taml" );
	TamlWrite( Ship, "modules/saveFiles/playerShip.taml" );
}
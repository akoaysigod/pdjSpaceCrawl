function createMainMenu() {
	exec( "./overworld.cs" );
	exec( "./gameScene.cs" );

	if ( isObject( GameScene ) ) {
		GameScene.delete();
	}

	if ( isObject( UIScene ) ) {
		UIScene.delete();
	}

	if ( isObject( UIWindow ) ) {
		UIWindow.delet();
	}

	if ( isObject( MainMenuScene ) ) {
		MainMenu.delete();
	}

	$musicChannel = 1;
	$sfxChannel = 2;

	alxSetChannelVolume( $musicChannel, 0.75 );
	alxSetChannelVolume( $sfxChannel, 1.0 );

	%spaceship = TamlRead( "modules/saveFiles/playerShip.taml" );
	Window.planetID = %spaceship.planetID;

	%main = new Scene( MainMenuScene );

	%main.musicID = alxPlay( "gameModule:mainMenuMusic" );

	%back = new Sprite() {
		image = "gameModule:mainMenuBackground";
		size = "100 75";
		position = "0 0";
		SceneLayer = 30;
	};
	%main.add( %back );

	%by = new ImageFont() {
		image = "gameModule:font";
		FontSize = 2;
		text = "by Anthony Green and";
		position = "-30 -33";
		textAlignmnet = "left";
	};
	%main.add( %by );

	%logo = new Sprite() {
		image = "gameModule:logo";
		size = "35 3";
		position = "-29.5 -36";
	};
	%main.add( %logo );

	%title = new Sprite() {
		image = "gameModule:title";
		size = "75 5";
		position = "0 33";
	};
	%title.setBlendMode( true );
	%title.setBlendColor( 0, 0, 1.0 );
	%main.add( %title );

	%new = new Sprite() {
		class = "Option";
		image = "gameModule:new";
		size = "10 5";
		position = "0 3";
		SceneLayer = 0;
		UseInputEvents = true;
	};
	%new.optionType = "newOption";
	%new.setBlendMode( true );
	%new.setBlendColor( 0, 0, 1.0 );
	%main.add( %new );

	%cont = new Sprite() {
		class = "Option";
		image = "gameModule:continue";
		size = "20 5";
		position = "0 -3";
		SceneLayer = 0;
		UseInputEvents = true;
	};
	%cont.optionType = "continueOption";
	%cont.setBlendMode( true );
	%cont.setBlendColor( 0, 0, 1.0 );
	%main.add( %cont );

	if ( %spaceship.hasGameOver ) {
		%cont.setUseInputEvents( false );
		%cont.setBlendAlpha( 0.5 );
	}

	Window.dismount();
	Window.setCameraPosition( 0, 0 );
	Window.setUseObjectInputEvents( true );
	Window.setScene( %main );
}

function Option::onTouchEnter( %this, %touchID, %pos ) {
	%s = new Sprite( Selector ) {
		image = "gameModule:planetSelector";
		position = %this.getPosition();
		SceneLayer = 1;
	};

	if ( %this.optionType $= "continueOption" ) {
		%s.size = %this.getSize();
	} else if ( %this.optionType $= "newOption" ) {
		%s.size = %this.getSize();
	}

	alxPlay( "gameModule:beep" );

	MainMenuScene.add( %s );
}

function Option::onTouchLeave( %this, %touchID, %pos ) {
	if ( isObject( Selector ) ) {
		Selector.delete();
	}
}

function Option::onTouchUp( %this, %touchID, %pos ) {
	alxPlay( "gameModule:highBeep" );
	
	if ( %this.optionType $= "continueOption" ) {
		createOverworld();
	} else if ( %this.optionType $= "newOption" ) {
		createScene();
	}

	alxStop( MainMenuScene.musicID );
}
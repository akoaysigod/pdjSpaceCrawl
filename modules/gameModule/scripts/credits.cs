function createCredits() {
	exec( "./mainMenu.cs" );

	MenuWindow.delete();
	UIWindow.delete();
	for ( %i = 0; %i != GameScene.getCount(); %i++ ) {
		%t = GameScene.getObject( %i );
		%t.safeDelete();
	}

	%credits = new Scene( CreditScene );

	%scrollSpeed = 10;

	%youWin = new ImageFont() {
		image = "gameModule:font";
		Text = "YOU WIN";
		FontSize = "10";
		Position = "50 -10";
		SceneLayer = "0";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%youwin.setBodyType( dynamic );
	%youwin.setLinearVelocityY( %scrollSpeed );
	%credits.add( %youWin );

	%tony = new ImageFont() {
		image = "gameModule:font";
		Text = "A game by Tony Green";
		FontSize = 3;
		Position = "50 -20";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%tony.setBodyType( dynamic );
	%tony.setLinearVelocityY( %scrollSpeed );
	%credits.add( %tony );

	%art = new ImageFont() {
		image = "gameModule:font";
		Text = "ART:";
		FontSize = 5;
		Position = "50 -27";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%art.setBodyType( dynamic );
	%art.setLinearVelocityY( %scrollSpeed );
	%credits.add( %art );

	%readme = new ImageFont() {
		image = "gameModule:font";
		Text = "see read me for full details. Sorry if I forgot someone =(";
		FontSize = 1;
		Position = "50 -33";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%readme.setBodyType( dynamic );
	%readme.setLinearVelocityY( %scrollSpeed );
	%credits.add( %readme );

	%master484 = new ImageFont() {
		image = "gameModule:font";
		Text = "Master484";
		FontSize = 3;
		Position = "50 -40";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%master484.setBodyType( dynamic );
	%master484.setLinearVelocityY( %scrollSpeed );
	%credits.add( %master484 );

	%thomaswsp = new ImageFont() {
		image = "gameModule:font";
		Text = "thomaswsp";
		FontSize = 3;
		Position = "50 -45";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%thomaswsp.setBodyType( dynamic );
	%thomaswsp.setLinearVelocityY( %scrollSpeed );
	%credits.add( %thomaswsp );

	%Jannax = new ImageFont() {
		image = "gameModule:font";
		Text = "Jannax";
		FontSize = 3;
		Position = "50 -50";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%Jannax.setBodyType( dynamic );
	%Jannax.setLinearVelocityY( %scrollSpeed );
	%credits.add( %Jannax );

	%CharlesGabriel = new ImageFont() {
		image = "gameModule:font";
		Text = "CharlesGabriel";
		FontSize = 3;
		Position = "50 -55";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%CharlesGabriel.setBodyType( dynamic );
	%CharlesGabriel.setLinearVelocityY( %scrollSpeed );
	%credits.add( %CharlesGabriel );

	%manenwolf = new ImageFont() {
		image = "gameModule:font";
		Text = "manenwolf";
		FontSize = 3;
		Position = "50 -60";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%manenwolf.setBodyType( dynamic );
	%manenwolf.setLinearVelocityY( %scrollSpeed );
	%credits.add( %manenwolf );

	%JustinNichol = new ImageFont() {
		image = "gameModule:font";
		Text = "Justin Nichol";
		FontSize = 3;
		Position = "50 -65";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%JustinNichol.setBodyType( dynamic );
	%JustinNichol.setLinearVelocityY( %scrollSpeed );
	%credits.add( %JustinNichol );

	%JustinNichol = new ImageFont() {
		image = "gameModule:font";
		Text = "Justin Nichol";
		FontSize = 3;
		Position = "50 -70";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%JustinNichol.setBodyType( dynamic );
	%JustinNichol.setLinearVelocityY( %scrollSpeed );
	%credits.add( %JustinNichol );

	%LordNeo = new ImageFont() {
		image = "gameModule:font";
		Text = "LordNeo";
		FontSize = 3;
		Position = "50 -75";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%LordNeo.setBodyType( dynamic );
	%LordNeo.setLinearVelocityY( %scrollSpeed );
	%credits.add( %LordNeo );

	%ac3raven = new ImageFont() {
		image = "gameModule:font";
		Text = "ac3raven";
		FontSize = 3;
		Position = "50 -80";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%ac3raven.setBodyType( dynamic );
	%ac3raven.setLinearVelocityY( %scrollSpeed );
	%credits.add( %ac3raven );

	%JustinCallaghan = new ImageFont() {
		image = "gameModule:font";
		Text = "JustinCallaghan";
		FontSize = 3;
		Position = "50 -85";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%JustinCallaghan.setBodyType( dynamic );
	%JustinCallaghan.setLinearVelocityY( %scrollSpeed );
	%credits.add( %JustinCallaghan );

	%music = new ImageFont() {
		image = "gameModule:font";
		text = "Music and Sound:";
		FontSize = 5;
		Position = "50 -95";
		TextAlignment = "center";
		LifeTime = 180;
	};
	%music.setBodyType( dynamic );
	%music.setLinearVelocityY( %scrollSpeed );
	%credits.add( %music );

	%logo = new Sprite() {
		image = "gameModule:logo";
		size = "35 3";
		position = "50 -103";
	};
	%logo.setBodyType( dynamic );
	%logo.setLinearVelocityY( %scrollSpeed );
	%credits.add( %logo );



	%background = new Sprite( CreditBack ); 
	%background.name = "back";
	%background.setUpdateCallback( true );
	%background.setBodyType( dynamic );
	%background.Position = "50 37.5";
	%background.Size = "100 75";
	%background.SceneLayer = 31;
	%background.Image = "gameModule:farBack";
	%credits.add( %background );
	
	%midground = new Sprite();
	%midground.name = "mid";
	%midground.setBodyType( dynamic );
	%midground.setBlendAlpha( 0.6 );
	%midground.Position = "48 33.5";
	%midground.Size = "100 75";
	%midground.SceneLayer = 30;
	%midground.Image = "gameModule:midBack";
	%credits.add( %midground );

	%foreground = new Sprite();
	%foreground.name = "fore";
	%foreground.setName( "foreground" );
	%foreground.setBodyType( dynamic );
	%foreground.setBlendAlpha( 0.4 );
	%foreground.Position = "52 39.5";
	%foreground.Size = "100 75";
	%foreground.SceneLayer = 29;
	%foreground.Image = "gameModule:foreBack";
	%credits.add( %foreground );

	%scrolling = new SceneObjectSet( BackSet );
	%scrolling.add( %background, %midground, %foreground );
	%back = new Sprite() {
		name = "back";
		size = "100 75";
		bodyType = dynamic;
		SceneLayer = 31;
	};

	%mid = new Sprite() {
		name = "mid";
		size = "100 75";
		bodyType = dynamic;
		SceneLayer = 30;
	};
	%mid.setBlendAlpha( 0.6 );

	%fore = new Sprite() { 
		name = "fore";
		size = "100 75";
		bodyType = dynamic;
		SceneLayer = 29;
	};
	%fore.setBlendAlpha( 0.4 );

	%back.copyFrom( %background );
	%back.position = "150 37.5";
	%mid.copyFrom( %midground );
	%mid.position = "154 33.5";
	%fore.copyFrom( %foreground );
	%fore.position = "157 39.5";

	%credits.add( %back );
	%credits.add( %mid );
	%credits.add( %fore );

	%scrolling.add( %back, %mid, %fore );
	
	%ship = new Sprite() {
		image = "gameModule:SpaceShip";
		size = "4 7";
		position = "10 37.5";
		angle = -90;
		SceneLayer = 27;
	};
	%ship.animation = "gameModule:shipAnim";
	%ship.setLinearVelocityX( 1 );
	%credits.add( %ship );

	Window.setScene( CreditScene );
	Window.setCameraPosition( 50, 37.5 );

	alxPlay( "gameModule:creditSong" );

	%input = new ScriptObject( GameOverInput );
	Window.addInputListener( %input );
	%controls = new ActionMap( GameOverControls );
	%controls.bindCmd( keyboard, "escape", "nothing();", "createMainMenu();" );
	%controls.push();
}

function CreditBack::onUpdate( %this ) {
	for ( %i = 0; %i != BackSet.getCount(); %i++ ) {
		%t = BackSet.getObject( %i );

		if ( %t.name $= "back" ) {
			%t.setLinearVelocityX( -10 );
		}

		if ( %t.name $= "mid" ) {
			%t.setLinearVelocityX( -11 );
		}

		if ( %t.name $= "fore" ) {
			%t.setLinearVelocityX( -12 );
		}

		%x = getWord( %t.getPosition(), 0 );
		if ( %x <= -150 ) {
			%t.position = 150 + getRandom( 1, 3 ) SPC 37.5 - getRandom( 1, 3 );
		}
	}
}
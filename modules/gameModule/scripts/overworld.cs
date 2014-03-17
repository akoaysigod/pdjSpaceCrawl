function createOverworld() {
	alxStopAll();

	if ( Window.planetID == 0 ) {
		%overworld = new Scene( OverworldScene );
		%circles = new CircleGen( Circles );
		%circles.createPlanets();

		%back = new Sprite() {
			image = "gameModule:overworldBackground";
			size = "100 75";
			position = "50 37.5";
			SceneLayer = 10;
		};
		%overworld.add( %back );
	} else {
		%overworld = TamlRead( "modules/saveFiles/overworld.taml" );

		for ( %i = 0; %i != %overworld.getCount(); %i++ ) {
			%t = %overworld.getObject( %i );
			if ( %t.current ) {
				%posX = getWord( %t.getPosition(), 0 );
				%posY = getWord( %t.getPosition(), 1 );
				%tmp = new ImageFont( Marker ) {
					image = "gameModule:font";
					Text = "HERE";
					FontSize = "2";
					Position = %posX SPC %posY + 2 + ( %t.getHeight / 2 );
					SceneLayer = "0";
					TextAlignment = "center";
				};
				%overworld.add( %tmp );
			}
		}
	}

	Window.planetID += 1;
	Window.setUseObjectInputEvents( true );
	Window.dismount();
	Window.setCameraPosition( 50, 37.5 );
	Window.setScene( %overworld );

	alxPlay( "gameModule:overworldTheme" );
}

function Circles::createPlanets( %this ) {
	%j = getRandom( 15, 20 );
	for ( %i = 0; %i != %j; %i++ ) {
		%chance = getRandom( 0, 2 );
		switch( %chance ) {
			case 0:
				%type = "gameModule:normalPlanet";
				%kind = "normal";
			case 1:
				%type = "gameModule:redPlanet";
				%kind = "red";
			case 2:
				%type = "gameModule:gasPlanet";
				%kind = "gas";
		}

		%planetSize = getRandom( 5, 10 );

		%free = false;
		while( !%free ) {
			%x = getRandom( 1, 9 );
			%y = getRandom( 1, 7 );
			%free = %this.pickCircle( %x, %y );
		}

		%posX = %x * 10;
		%posY = %y * 10;

		%rX = getRandom( 1, 3 );
		if ( getRandom( 0, 1 ) ) {
			%rX *= -1;
		}
		%rY = getRandom( 1, 3 );
		if ( getRandom( 0, 1 ) ) {
			%rY *= -1;
		}

		%posX -= %rX;
		%posX += %rY;
		
		%planet = new Sprite() {
			class = "Planet";
			image = %type;
			size = %planetSize SPC %planetSize;
			position = %posX SPC %posY;
			UseInputEvents = true;
		};
		%planet.kind = %kind;

		if ( %i == 0 ) {
			%planet.current = true;
			%planet.visited = true;
			%planet.setBlendAlpha( 0.5 );

			%tmp = new ImageFont( Marker ) {
				image = "gameModule:font";
				Text = "HERE";
				FontSize = "2";
				Position = %posX SPC %posY + 2 + ( %planet.getHeight / 2 );
				SceneLayer = "0";
				TextAlignment = "center";
			};
			OverworldScene.add( %tmp );
		} else {
			%planet.current = false;
			%planet.visited = false;
		}
		OverworldScene.add( %planet );
	}
}

function Planet::onTouchEnter( %this, %touchID, %pos ) {
	alxPlay( "gameModule:beep" );

	if ( %this.getBlendAlpha() != 1.0 ) {
		return;
	}

	%s = %this.getWidth();

	%selection = new Sprite( Selector ) {
		image = "gameModule:planetSelector";
		size = %s SPC %s;
		position = %this.getPosition();
		SceneLayer = 1;
	};
	OverworldScene.add( %selection );
}

function Planet::onTouchLeave( %this, %touchID, %pos ) {
	Selector.delete();
}

function Planet::onTouchUp( %this, %touchID, %pos ) {
	alxPlay( "gameModule:highBeep" );

	alxStopAll();
	
	if ( %this.getBlendAlpha() != 1.0 ) {
		return;
	}
	
	if ( isObject( Selector ) ) { 
		Selector.delete();
	}

	if ( isObject( Marker ) ) {
		Marker.delete();
	}

	for ( %i = 0; %i != OverworldScene.getCount(); %i++ ) {
		%t = OverworldScene.getObject( %i );
		if ( %t.current ) {
			%t.current = false;
			%this.current = true;
			%this.visited = true;
			%this.setBlendAlpha( 0.5 );
		}
	}

	TamlWrite( OverworldScene, "modules/saveFiles/overworld.taml" );

	Window.useObjectInputEvents = false;

	createScene();
	Window.setScene( GameScene );
	createWorld();
}

function removeOverworld() {
	OverworldScene.clear( true );
	OverworldScene.delete();
}
function createOverworld() {
	%overworld = new Scene( OverworldScene );

	%circles = new CircleGen( Circles );
	//%circles.circlePack();
	%circles.createPlanets();

	Window.useObjectInputEvents = true;
	Window.dismount();
	Window.setCameraPosition( 0, 0 );
	Window.setScene( %overworld );
}

function Circles::createPlanets( %this ) {
	%j = getRandom( 15, 20 );
	for ( %i = 0; %i != %j; %i++ ) {
		%chance = getRandom( 0, 2 );
		switch( %chance ) {
			case 0:
				%type = "gameModule:normalPlanet";
				%name = "normal";
			case 1:
				%type = "gameModule:redPlanet";
				%name = "red";
			case 2:
				%type = "gameModule:gasPlanet";
				%name = "gas";
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
		echo( %x SPC %y );
		if ( %x % 2 == 0 ) {
			%posX -= getRandom( 1, 3 );
		} else {
			%posX += getRandom( 1, 3 );
		}

		%planet = new Sprite( Planet ) {
			name = %name;
			image = %type;
			size = %planetSize SPC %planetSize;
			position = %posX SPC %posY;
			UseInputEvents = true;
		};
		if ( %i == 0 ) {
			%planet.visited = true;
			%planet.setBlendAlpha( 0.5 );
		} else {
			%planet.visited = false;
		}
		OverworldScene.add( %planet );
	}
}

function Planet::onTouchEnter( %this, %touchID, %pos ) {
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
	Selector.safeDelete();
}

function Planet::onTouchUp( %this, %touchID, %pos ) {
	if ( isObject( Selector ) ) { 
		Selector.safeDelete();
	}

	if ( %this.getBlendAlpha() != 1.0 ) {
		return;
	}


	TamlWrite( OverworldScene, "modules/saveFiles/overworld.taml" );

	Window.useObjectInputEvents = false;

	createScene();
	Window.setScene( GameScene );
	createWorld();
}

function removeOverworld() {
	OverworldScene.clear( true );
}
function createMenuWindow() {
	pause();

	%tmpWindow = new SceneWindow( MenuWindow ) {
		Profile = GuiDefaultProfile;
		size = "100 75";
		position = "0 0";
	};
	Canvas.add( %tmpWindow );

	%input = new ScriptObject( InputManager );
	%tmpWindow.addInputListener( %input );

	%tmpScene = new Scene( MenuScene );

	%background = new Sprite( MenuBox ) {
		image = "gameModule:messageBox";
		size = "50 30";
		position = "0 0";
		SceneLayer = 5;
		angle = 90;
	};
	%tmpScene.add( %background );

	%tmpWindow.setScene( %tmpScene );

	%background.textTimer();
}

function Character::typeText( %this ) {
	%this.visible = true;
}

function MenuBox::textTimer( %this ) {
	%time = 50;
	%char = new ImageFont( Character ) {
		image = "gameModule:font";
		text = "Mothership";
		fontSize = "2";
		position = "0 23";
		textAlignment = "center";
		SceneLayer = 0;
		visible = false;
	};
	MenuScene.add( %char );
	Character.startTimer( typeText, %time, 1 );

	%time += 50;

	%yOff = 20;
	for ( %i = 0; %i != 5; %i++ ) {
		switch( %i ) {
			case 0:
				%text = "1. Repair Pod";
			case 1:
				%text = "2. Change weapon";
			case 2:
				%text = "3. Change systems";
			case 3:
				%text = "4. Lift off";
			case 4:
				%text = "5. Jettison Pod";
		}

		%option = new ImageFont( Character ) {
			image = "gameModule:font";
			text = %text;
			fontSize = "1.5";
			position = "-13" SPC %yOff;
			textAlignment = "left";
			SceneLayer = 0;
			visible = false;
		};
		MenuScene.add( %option );
		Character.startTimer( typeText, %time, 1 );
		%time += 50;
		%yOff -= 5;
	}

	MenuBox.startTimer( setupControls, %time + 50, 1 );
}

function MenuBox::setupControls( %this ) {
	%controls = new ActionMap( menuControls );
	%controls.bindCmd( keyboard, "5", "nothing();", "MenuBox.jettison();" );
	%controls.push();
}

function MenuBox::jettison( %this ) {
	unpause();
	schedule( 10, 0, deleteMenuBox );
}

function deleteMenuBox() {
	MenuScene.clear( true );
	menuControls.pop();
	InputManager.delete();
	MenuWindow.delete();

	unpause();

	Ship.health = 100;
	HealthBar.updateHealth( 0 );
	Ship.setLinearVelocityY( 25 );
}







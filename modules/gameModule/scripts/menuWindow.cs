function createMenuWindow() {
	exec( "./overworld.cs" );

	pause();

	if ( isObject( OverworldScene ) ) {
		removeOverworld();
	}

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
		%alpha = 1;
		%label = " ";
		switch( %i ) {
			case 0:
				%label = "repairPod";
				if ( Ship.health == 100 ) {
					%text = "1.Repair Pod";
					%alpha = 0.5;
				} else { 
					%text = "1.Repair Pod" SPC mCeil( 100 - Ship.health );
				}
			case 1:
				%label = "repairShip";
				if ( Mothership.health == 100 ) {
					%text = "2.Repair Ship";
					%alpha = 0.5;
				} else {
					%text = "2.Repair Ship" SPC mCeil( 100 - Mothership.health ) * 2;
				}
			case 2:
				%text = "3.Make ammo";
				if ( !Ship.hasSpecial ) {
					%alpha = 0.5;
				}
			case 3:
				%text = "4.Lift off";
				if ( Mothership.fuel < 4 || GameScene.hasMotherPart ) {
					%alpha = 0.5;
				}
			case 4:
				%text = "5.Jettison Pod";
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
		%option.label = %label;
		%option.setBlendAlpha( %alpha );
		MenuScene.add( %option );

		Character.startTimer( typeText, %time, 1 );
		%time += 50;
		%yOff -= 5;
	}

	MenuBox.startTimer( setupControls, %time + 50, 1 );
}

function MenuBox::setupControls( %this ) {
	%controls = new ActionMap( menuControls );
	%controls.bindCmd( keyboard, "1", "nothing();", "MenuBox.repair();" );
	%controls.bindCmd( keyboard, "2", "nothing();", "MenuBox.repairShip();" );
	%controls.bindCmd( keyboard, "3", "nothing();", "MenuBox.weapon();" );
	%controls.bindCmd( keyboard, "4", "nothing();", "MenuBox.liftOff();" );
	%controls.bindCmd( keyboard, "5", "nothing();", "MenuBox.jettison();" );
	%controls.push();
}

function MenuBox::repair( %this ) {
	%cost = mCeil( 100 - Ship.health );
	echo( "wtf" );
	if ( Mothership.money > %cost ) {
		Ship.health = 100;
		HealthBar.updateHealth( 0 );
		Mothership.money -= %cost;
		MoneyLabel.updateMoney( %cost * -1 );

		for ( %i = 0; %i != MenuScene.getCount(); %i++ ) {
			%t = MenuScene.getObject( %i );
			if ( %t.label $= "repairPod" ){
				%t.text = "1.Repair Pod";
				%t.setBlendAlpha( 0.5 );
			}
		}
	}
}

function MenuBox::repairShip( %this ) {
	%cost = mCeil( 100 - Mothership.health ) * 2;
	if ( Mothership.money > %cost ) {
		Mothership.health = 100;
		MotherHealth.updateHealth( 0 );
		Mothership.money -= %cost;
		MoneyLabel.updateMoney( %cost * -1 );

		for ( %i = 0; %i != MenuScene.getCount(); %i++ ) {
			%t = MenuScene.getObject( %i );
			if ( %t.label $= "repairShip" ){
				%t.text = "2.Repair Ship";
				%t.setBlendAlpha( 0.5 );
			}
		}
	}
}

function MenuBox::weapon( %this ) {

}

function MenuBox::system( %this ) {

}

function MenuBox::liftOff( %this ) {
	if ( Mothership.fuel < 4 ) {
		return;
	}

	if ( GameScene.hasMotherPart ) {
		return;
	}

	Mothership.fuel = 0;

	saveGame();
	
	createOverworld();

	GameScene.delete();
	removePlayerUI();
	deleteMenuBox();
}

function MenuBox::jettison( %this ) {
	unpause();
	schedule( 10, 0, deleteMenuBox );
}

function deleteMenuBox() {
	MenuScene.clear( true );
	//delete menuscene?
	menuControls.pop();
	InputManager.delete();
	MenuWindow.delete();

	unpause();

	if ( isObject( Ship ) ) {
		Ship.setLinearVelocity( "0 30" );
		Ship.setLinearDamping( 1 );
	}
}







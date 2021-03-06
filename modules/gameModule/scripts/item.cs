function createItem( %itemType ) { 
	%item = new Sprite( Item );
	switch$ ( %itemType ) {
		case "fuel":
			%item.name = "fuelItem";
			%item.image = "gameModule:fuel";
			%item.size = "4 7";
			%item.weight = "0 -1";
			%item.text = "This is fuel for the mothership.";

		case "shields":
			%item.name = "shieldItem";
			%item.image = "gameModule:shields";
			%item.size = "5 5";
			%item.weight = "0 -3";
			%item.text = "You found a shield generator for your POD. Press Q to activate.";

		case "shipPartOne":
			%item.name = "partOneItem";
			%item.image = "gameModule:shipPartOne";
			%item.size = "5 6";
			%item.weight = "0 -2";
			%item.text = "A piece of the Mothership. You'll need all four to get home (win). This one strengthens the defenses of the Mothership.";

		case "shipPartTwo":
			%item.name = "partTwoItem";
			%item.size = "5 6";
			%item.weight = "0 -2";
			%item.text = "You found a piece of the Mothership. This is an orbital repair unit. It'll allow you to repair some damage while travelling between planets.";

		case "shipPartThree":
			%item.name = "partThreeItem";
			%item.size = "5 6";
			%item.weight = "0 -2";
			%item.text = "The manufacturing bay allows the Mothership to create ammo for free.";

		case "shipPartFour":
			%item.name = "partFourItem";
			%item.size = "5 6";
			%item.weight = "0 -2";
			%item.text = "The Mothership engine part greatly increases fuel efficiency. Now only three fuel are required to leave a planet.";

		case "reverseThrusters":
			%item.name = "reverseThrusters";
			%item.image = "gameModule:shields";
			%item.size = "5 5";
			%item.weight = "0 -2.5";
			%item.text = "The reverse thrusters allow you to slow down in low gravity situations. Press the S key to use.";

		case "missiles":
			%item.name = "missiles";
			%item.image = "gameModule:shields";
			%item.size = "5 5";
			%item.weight = "0 -1";
			%item.text = "You have found missiles. You may no manufacture ammo in the Mothership. Press E to shoot.";

		case "boosters":
			%item.name = "boosters";
			%item.image = "gameModule:shields";
			%item.size = "5 5";
			%item.weight = "0 -2";
			%item.text = "These boosters will help you carry back heavy stuff or give you a quick boost to get going. Press R to use boosters";
	}

	%item.text = %item.text SPC  "Press ENTER to continue. ";

	%item.SceneLayer = 5;
	%item.setBodyType( dynamic );
	%item.createPolygonBoxCollisionShape();
	%item.setCollisionLayers(  1, 2 );
	%item.setCollisionGroups( 1, 2 );
	%item.setSceneGroup( 25 );	
	%item.setUpdateCallback( true );
	%item.setFixedAngle( true );

	%item.joint = -1;
	%item.isAttached = false;

	return %item;
}

function Item::findAngle( %this, %angleTo ) {
	%angle = Vector2AngleToPoint( %angleTo, %this.getPosition() );
	%this.line.setAngle( %angle );
}

function Item::onUpdate( %this ) { 
	%dist = VectorDist( %this.getPosition(), Ship.getPosition() ); 

	if ( Ship.isAttached && %this.isAttached ) {
		%this.findAngle( Ship.getPosition() );
		%this.line.position = %this.getPosition();
		%this.applyLinearImpulse( %this.weight, %this.getPosition() );
	}

	if ( %dist < 11 && %this.joint == -1 && !Ship.isAttached && !Ship.shieldsActive ) {
		%this.joint = GameScene.createDistanceJoint( Ship, %this, "0 0", "0 0", 10, 0.0, 1.0, false );
		Ship.isAttached = true;
		%this.isAttached = true;

		%this.line = new ShapeVector();
		%this.line.PolyList = "0 10 0.1 10 0.1 0 0 0";
		%this.line.position = %this.getPosition();
		%this.line.SceneLayer = 6;
		%this.findAngle( Ship.getPosition() );
		%this.line.position = %this.getPosition();
		GameScene.add( %this.line );
	}

	if ( Ship.shieldsActive && %this.isAttached ) {
		GameScene.deleteJoint( %this.joint );
		Ship.isAttached = false;
		%this.isAttached = false;

		%this.setLinearVelocity( "0 0" );
		%this.line.safeDelete();
		%this.joint = -1;
	}
	
	if ( getWord( %this.getPosition(), 1 ) > getWord( Mothership.getPosition(), 1 ) + 15 ) {
		if ( mAbs( getWord( %this.getPosition(), 0 ) - getWord( Mothership.getPosition(), 0 ) ) < 5 ) {
			if ( %this.joint != -1 ) {
				Ship.isAttached = false;
				%this.isAttached = false;

				%wtf = GameScene.deleteJoint( %this.joint );
				%this.setLinearVelocity( "0 -5" );
				%this.setUpdateCallback( false );
				%this.line.safeDelete();
			}
		}
	}
}
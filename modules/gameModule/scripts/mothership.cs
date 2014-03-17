function createMothership() {
	exec( "./MessageWindow.cs" );

	if ( Window.planetID == 0 ) {
		%mothership = TamlRead( "modules/defaultFiles/mothership.taml" );
		%mothership.hasUpgradeOne = false;
		%mothership.hasUpgradeTwo = false;
		%mothership.hasUpgradeThree = false;
		%mothership.hasUpgradeFour = false;
		%mothership.health = 100;
		%mothership.money = 0;
	} else {
		%mothership = TamlRead( "modules/saveFiles/mothership.taml");
	}
	%mothership.fuel = 0;
	%mothership.attackMusic = -1;

	return %mothership;
}

function Mothership::createDropBox( %this ) {
	%dropBox = new SceneObject( DropBox );
	%dropBox.name = "dropBox";
	%dropBox.setBodyType( static );
	%dropBox.size = "25 10";
	%dropBox.createPolygonBoxCollisionShape();
	%dropbox.setPositionX( getWord( %this.getPosition(), 0 ) );
	%dropbox.setPositionY( getWord( %this.getPosition(), 1 ) - 5 );
	%dropbox.setCollisionGroups( 13, 25 );
	%dropbox.setCollisionLayers( 13, 25 );
	%dropBox.setSceneGroup( 2 );
	%dropBox.setCollisionCallback( true );
	
	GameScene.add( %dropBox );
}

function Mothership::takeDamage( %this, %dmg ) {
	if ( %this.hasUpgradeOne ) {
		MotherHealth.updateHealth( %dmg / 10 );
	} else { 
		MotherHealth.updateHealth( %dmg / 5);
	}

	if ( !isObject( Alert ) ) {
		MotherHealth.setupAlert();
	}

	if ( !alxIsPlaying( %this.attackMusic ) ) {
		alxStopAll();
		%this.attackMusic = alxPlay( "gameModule:mothershipUnderAttack" );
	}
	
}

function DropBox::onCollision( %this, %collides, %details ) {
	if ( %collides.name $= "enemyBullet" ) {
		Mothership.takeDamage( %collides.damage );
		%collides.safeDelete();
	}

	switch$ ( %collides.name ) {
		case "fuelItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.fuel += 1;
			FuelBar.updateFuel();
			alxPlay( "gameModule:typing" );

		case "shieldItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Ship.hasShields = true;
			alxPlay( "gameModule:typing" );

		case "partOneItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeOne = true;
			GameScene.hasMotherPart = false;
			alxPlay( "gameModule:typing" );

		case "partTwoItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeTwo = true;
			GameScene.hasMotherPart = false;
			alxPlay( "gameModule:typing" );

		case "partThreeItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeThree = true;
			GameScene.hasMotherPart = false;
			alxPlay( "gameModule:typing" );

		case "partFourItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeFour = true;
			GameScene.hasMotherPart = false;
			alxPlay( "gameModule:typing" );

		case "reverseThrusters":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Ship.hasReverseThrusters = true;
			alxPlay( "gameModule:typing" );

		case "missiles":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Ship.hasSpecial = true;
			alxPlay( "gameModule:typing" );

		case "boosters":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Ship.hasBoosters = true;
			alxPlay( "gameModule:typing" );
	}
}

function Mothership::onCollision( %this, %collides, %details ) {
	if ( %collides.name $= "enemyBullet" ) {
		%this.takeDamage( %collides.damage );
		%collides.safeDelete();
	}
}

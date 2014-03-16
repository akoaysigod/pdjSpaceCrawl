function createMothership() {
	exec( "./MessageWindow.cs" );

	if ( Window.planetID == 0 ) {
		%mothership = TamlRead( "modules/defaultFiles/mothership.taml" );
		%mothership.hasUpgradeOne = false;
		%mothership.hasUpgradeTwo = false;
		%mothership.hasUpgradeThree = false;
		%mothership.hasUpgradeFour = false;
		%mothership.health = 100;
		%mothership.money = 1000;
	} else {
		%mothership = TamlRead( "modules/saveFiles/mothership.taml");
	}
	%mothership.fuel = 0;

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

		case "shieldItem":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Ship.hasShields = true;

		case "shipPartOne":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeOne = true;
			GameScene.hasMotherPart = false;

		case "shipPartTwo":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeTwo = true;
			GameScene.hasMotherPart = false;

		case "shipPartThree":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeThree = true;
			GameScene.hasMotherPart = false;

		case "shipPartFour":
			messageWindowCreate( %collides.text );
			%collides.safeDelete();
			Mothership.hasUpgradeFour = true;
			GameScene.hasMotherPart = false;
	}
}

function Mothership::onCollision( %this, %collides, %details ) {
	if ( %collides.name $= "enemyBullet" ) {
		%this.takeDamage( %collides.damage );
		%collides.safeDelete();
	}
}













/*
	%mothership = new Sprite( Mothership );
	%mothership.name = "mothership";
	%mothership.Image = "gameModule:mothership";
	%mothership.SceneLayer = 0;
	%mothership.setBodyType( static );
	%mothership.createEdgeCollisionShape( -15, -12.5, 15, -12.5 );
	%mothership.createEdgeCollisionShape( -15, -12.5, -15, 4.5 );
	%mothership.createEdgeCollisionShape( 15, -12.5, 15, 4.5 );
	%mothership.createEdgeCollisionShape( -15, 4.5, -9, 12.5 );
	%mothership.createEdgeCollisionShape( 15, 4.5, 9, 12.5 );
	
	%mothership.setDefaultRestitution( 0.5 );
	%mothership.setSceneGroup( 1 );
	%mothership.setCollisionGroups( 1, 13 );
	%mothership.setCollisionLayers( 1, 13 );
	%mothership.setCollisionCallback( true );
	%mothership.Size = "30 25";

	%mothership.fuel = 0;
*/
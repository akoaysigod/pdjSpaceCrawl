function createSpaceShip() {
	exec( "./playerUI.cs" );
	exec( "./menuWindow.cs" );
	exec( "./shields.cs" );

	if ( Window.planetID == 0 ) {
		%spaceship = TamlRead( "modules/defaultFiles/playerShip.taml");
		%spaceship.hasGameOver = false;
		%spaceship.hasShields = false;
		%spaceship.shieldsActive = false;
	} else {
		%spaceship = TamlRead( "modules/saveFiles/playerShip.taml" );
	}

	%spaceship.isAttached = false;
	
	createPlayerUI();

	return %spaceship;
}

function Ship::onCollision( %this, %collides, %collisionDetails ) {
	%change = -1000;
	if ( %collides.name $= "enemyBullet" ) {
		%change = %collides.damage;
		%collides.safeDelete();
	} else if ( %collides.name $= "block" || %collides.name $= "mothership" ) {
		%change = VectorLen( %this.getLinearVelocity() ) / 5;
		%change = mFloor( %change );
		if ( %change == 0 ) {
			%change = 0.5;
		}
	} else if ( %collides.name $= "dropbox" ) {
		%this.openShipMenu();
	}

	HealthBar.updateHealth( %change );
}

function Ship::openShipMenu( %this ) {
	%this.setAngle( 0 );
	%this.setPositionX( getWord( Mothership.getPosition(), 0 ) );

	createMenuWindow();

}

function Ship::onUpdate( %this ) { 

}













/*
	%spaceship = new Sprite( Ship );
	%spaceship.name = "ship";
	%spaceship.setBodyType( dynamic );
	%spaceship.Position = "0 20";
	%spaceship.Size = "4 7";
	%spaceship.SceneLayer = 1;
	%spaceship.Image = "gameModule:SpaceShip";
	%spaceship.createPolygonBoxCollisionShape(4, 5, 0, 1);
	%spaceship.setCollisionCallback( true );
	%spaceship.setSceneGroup( 0 );
	%spaceship.setCollisionLayers( 1, 2, 10, 12, 13 );
	%spaceship.setCollisionGroups( 1, 2, 10, 12, 13 );
	%spaceship.setFixedAngle( true );
	%spaceship.setUpdateCallback( true );

	%spaceship.health = 100;
	%spaceship.fireRate = 5;
	%spaceship.rechargeRate = 250;
*/
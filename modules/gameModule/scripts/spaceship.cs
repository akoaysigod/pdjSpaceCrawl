function createSpaceShip() {
	exec( "./playerUI.cs" );

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
	%spaceship.health = 100;

	createPlayerUI();

	return %spaceship;
}

function Ship::onCollision( %this, %collides, %collisionDetails ) {
	echo( %collides.name );
	%change = 0;
	if ( %collides.name $= "enemyBullet" ) {
		%change = %collides.damage;
		%collides.safeDelete();
	} else if ( %collides.name $= "block" || %colldes.name $= "mothership" ) {
		%change = VectorLen( %this.getLinearVelocity() / 4 );
	}

	HealthBar.updateHealth( %change );
}


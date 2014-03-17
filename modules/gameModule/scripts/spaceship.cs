function createSpaceShip() {
	exec( "./playerUI.cs" );
	exec( "./menuWindow.cs" );
	exec( "./shields.cs" );

	if ( isObject( Ship ) ) {
			Ship.delete();
	}

	if ( Window.planetID == 0 ) {
		%spaceship = TamlRead( "modules/defaultFiles/playerShip.taml");
		%spaceship.hasGameOver = false;
		%spaceship.hasShields = false;
		%spaceship.shieldsActive = false;
		%spaceship.hasBoosters = false;
		%spaceship.hasReverseThruster = false;
		%spaceship.hasSpecial = false;
		%spaceship.planetID = 0;
		%spaceship.ammo = 10;
	} else {
		%spaceship = TamlRead( "modules/saveFiles/playerShip.taml" );
	}

	%spaceship.isAttached = false;
	
	createPlayerUI();

	return %spaceship;
}

function Missile::onCollision( %this, %collides, %details ) {
	if ( %collides.kind $= "enemy" ) {
		%collides.takeDamage( %this.dmg );
		%this.safeDelete();
	} else if ( %collides.name $= "border" ) {
		%this.safeDelete();
	}
}

function Ship::launchMissile( %this ) {
	if ( Ship.ammo <= 0 ) {
		return;
	}

	Ship.ammo -= 1;
	AmmoLabel.updateAmmo();

	%missile = new Sprite() {
		class = "Missile";
		image = "gameModule:missile";
		size = "2 5";
		position = %this.getPosition();
		SceneGroup = 7;
		SceneLayer = 7;
		LifeTime = 10;
	};
	%missile.dmg = 100;
	%missile.setCollisionCallback( true );
	%missile.createPolygonBoxCollisionShape( 2, 5 );
	%missile.setCollisionGroups( 11, 12 );
	%missile.setUpdateCallback( true );

	%adjustedAngle = %this.getAngle();

	if ( %adjustedAngle < 0 ) {
			%adjustedAngle *= -1;
	} else if ( %adjustedAngle > 0 ) {
			%adjustedAngle = 360 - %adjustedAngle;
	}
	
	
	%shootDir = Vector2Direction( %adjustedAngle, 50 );

	%missile.setAngle( %this.getAngle() );

	%missile.setLinearVelocity( %shootDir );

	alxPlay( "gameModule:missileLaunch" );
	
	GameScene.add( %missile );
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
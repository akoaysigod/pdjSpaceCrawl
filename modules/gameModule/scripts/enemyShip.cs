function createEnemyShip() {
	%enemy = new Sprite( Enemy );
	%enemy.name = "enemy";
	%enemy.Image = "gameModule:enemyShip";
	%enemy.SceneLayer = 3;
	%enemy.Position = "10 25";
	%enemy.Size = "7 4";
	%enemy.setBodyType( dynamic );
	%enemy.createPolygonBoxCollisionShape();
	%enemy.setCollisionLayers( 0, 1, 2 );
	%enemy.setCollisionGroups( 0, 1, 2 );
	%enemy.setSceneGroup( 12 );
	%enemy.setFixedAngle( true );
	%enemy.setDefaultRestitution( 0.5 );
	
	%enemy.setUpdateCallback( true );
	%enemy.following = false;

	%enemy.health = 2;

	return %enemy;
}

function Enemy::onUpdate( %this ) {
	%dist = VectorDist( %this.getPosition(), Ship.getPosition() );

	if ( %dist < 30 && !%this.following ) { 
		%this.following = true;
	} 

	if ( %this.following && %dist > 20 ) {
		%this.moveTo( Ship.getPosition(), 10 );
	} else {
		%this.cancelMoveTo();
	}

	if ( %this.following && !%this.isTimerActive() ) {
		%this.startTimer( fireShot, 750, 0 );
	} else if ( %enemy.following && %dist > 20 && %dist < 30 ) {
		%this.stopTimer();
	}
}

function Enemy::fireShot( %this ) {
	%bullet = new Sprite();
	%bullet.Name = "enemyBullet";
	%bullet.Image = "gameModule:enemyBullet";
	%bullet.Size = "2 2";
	%bullet.createCircleCollisionShape( 2 );
	%bullet.setCollisionGroups( 30 );
	%bullet.setCollisionLayers( 30 );
	%bullet.setSceneGroup( 13 );
	//%bullet.setDefaultRestitution( 0.5 );
	%bullet.Position = %this.getPosition();

	%bullet.damage = 3;

	%shootDir = Vector2AngleToPoint( Ship.getPosition(), %this.getPosition() );
	if ( %shootDir < 0 ) {
			%shootDir *= -1;
	} else if ( %shootDir > 0 ) {
			%shootDir = 360 - %shootDir;
	}

	%shootVel = Vector2Direction( %shootDir, 30 );

	GameScene.add( %bullet );

	%bullet.setLinearVelocity( %shootVel );
} 

function Enemy::takeDamage( %this, %damage ) {
	%this.health -= %damage;
	echo( %this.health );
	if ( %this.health <= 0 ) {
		%this.safeDelete();
	}
}
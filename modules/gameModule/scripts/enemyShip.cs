function createEnemyShip() {
	%enemy = new Sprite( Enemy );
	%enemy.name = "enemy";
	%enemy.Image = "gameModule:enemyShip";
	%enemy.kind = "enemy";
	%enemy.SceneLayer = 3;
	%enemy.Position = "10 25";
	%enemy.Size = "7 4";
	%enemy.setBodyType( dynamic );
	%enemy.createPolygonBoxCollisionShape();
	%enemy.setCollisionLayers( 0, 1, 2, 7 );
	%enemy.setCollisionGroups( 0, 1, 2, 7 );
	%enemy.setSceneGroup( 12 );
	%enemy.setFixedAngle( true );
	%enemy.setDefaultRestitution( 0.5 );
	
	%enemy.setUpdateCallback( true );
	%enemy.following = false;
	%enemy.mother = false;
	%enemy.isPatrolling = false;

	%enemy.fireRate = 1000 - ( Window.planetID * 50 );
	%enemy.speed = 10 + Window.planetID;
	%enemy.health = 1 * ( 2 + Window.planetID );
	%enemy.movePos = -1;

	%enemy.value = 10 + ( Window.planetID * 10 );

	return %enemy;
}

function Enemy::reactivateTimer( %this ) {
	%this.startTimer( fireShot, %this.fireRate, 0 );
}

function Enemy::hoverMode( %this ) {
	%x = getWord( %this.getPosition(), 0 );
	%y = getWord( %this.getPosition(), 1 );

	%xM = getWord( Mothership.getPosition(), 0 );
	%yM = getWord( Mothership.getPosition(), 1 );

	%coin = getRandom( 0, 1 );
	%yMove = getRandom( 1, 3 );
	if ( %coin ) {
		%yMove *= -1;
	}
	%y += %yMove;

	if ( %y < %yM + 15 ) {
		%y = %yM + 20;
	}

	if ( %x < %xM ) {
		%x = %xM + 20;
		%this.moveTo( %x SPC  %y, %this.speed, false, false );
	} else { 
		%x = %xM - 20;
		%this.moveTo( %x SPC %y, %this.speed, false, false );
	}

	%this.movePos = %x SPC %y;
}

function Enemy::patrol( %this ) {
	%this.isPatrolling = true; 

	%x = getWord( %this.getPosition(), 0 );
	if ( %x > getWord( Mothership.getPosition(), 0 ) ) {
		%this.setLinearVelocityX( %this.speed * -1 );
	} else { 
		%this.setLinearVelocityX( %this.speed );
	}
}

function Enemy::attackMother( %this ) {
	%x = getWord( Mothership.getPosition(), 0 );
	%y = getWord( Mothership.getPosition(), 1 );

	%spawnX = getWord( %this.getPosition(), 0 );
	if ( %spawnX < %x ) {
		%x -= getRandom( 50, 75 );
	} else {
		%x += getRandom( 50, 75 );
	}

	%pos = %x SPC %y + getRandom( 25, 50 );

	%this.moveTo( %pos, %this.speed, false, false );
	%this.movePos = %pos;

	%this.mother = true;
}

function BulletCall::onCollision( %this, %collides, %details ) {
	echo( %collides.name );
	if ( %collides.name $= "border" ) {
		%this.safeDelete();
	}
}

function Enemy::fireShot( %this ) {
	%bullet = new Sprite( BulletCall );
	%bullet.Name = "enemyBullet";
	%bullet.Image = "gameModule:enemyBullet";
	%bullet.Size = "2 2";
	%bullet.createCircleCollisionShape( 2 );
	%bullet.setCollisionGroups( 20, 30 );
	%bullet.setCollisionLayers( 20, 30 );
	%bullet.setSceneGroup( 13 );
	%bullet.setLifeTime( 600 );
	%bullet.sceneLayer = 10;
	//%bullet.setDefaultRestitution( 0.5 );
	%bullet.Position = %this.getPosition();
	%bullet.setCollisionCallback( true );

	%bullet.damage = 3 + Window.planetID;

	if ( %this.following ) {
		%shootDir = Vector2AngleToPoint( Ship.getPosition(), %this.getPosition() );
	} else {
		%shootDir = Vector2AngleToPoint( Mothership.getPosition(), %this.getPosition() );
	}

	if ( %shootDir < 0 ) {
			%shootDir *= -1;
	} else if ( %shootDir > 0 ) {
			%shootDir = 360 - %shootDir;
	}

	%shootVel = Vector2Direction( %shootDir, 30 );

	GameScene.add( %bullet );

	%bullet.setLinearVelocity( %shootVel );

	alxPlay( "gameModule:enemyLaserSound" );
} 

function Enemy::takeDamage( %this, %damage ) {
	%this.health -= %damage;
	if ( %this.health <= 0 ) {
		alxPlay( "gameModule:explosion" );
		MoneyLabel.updateMoney( %this.value );

		%explosion = new ParticlePlayer();
		%explosion.Particle = "gameModule:impactExplosion";
		%explosion.position = %this.getPosition();
		%explosion.setSizeScale( 1 );
		GameScene.add( %explosion );

		%this.safeDelete();
	}
}

function Enemy::updateMother( %this ) {
	%distMother = VectorDist( %this.getPosition(), Mothership.getPosition() );

	if ( %this.mother && %this.isMoveToComplete() && !%this.isPatrolling ) {
		%this.patrol();
	}

	if ( %distMother < 50 && !%this.isTimerActive() ) {
		%this.startTimer( fireShot, %this.fireRate, 0 );
		%this.setLinearVelocity( 0, 0 );
		%this.hoverMode();
		return;
	}

	if ( %this.isMoveToComplete() ) {
		%this.hoverMode();
	}
}

function Enemy::onUpdate( %this ) {
	%dist = VectorDist( %this.getPosition(), Ship.getPosition() );

	if ( %dist < 30 && !%this.following ) { 
		%this.following = true;
	} 

	if ( %this.mother ) {
		%this.updateMother();
		return;
	}

	if ( %this.following && %dist > 20 ) {
		%this.moveTo( Ship.getPosition(), %this.speed, false, false );
		%this.movePos = Ship.getPosition();
	} else {
		%this.cancelMoveTo();
	}

	if ( %this.following && !%this.isTimerActive() ) {
		%this.startTimer( fireShot, %this.fireRate, 0 );
	} else if ( %this.following && %dist > 30 ) {
		%this.stopTimer();
	} 
}
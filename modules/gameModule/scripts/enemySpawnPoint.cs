function createEnemySpawnPoint( %xPos, %yPos ) {
	%spawn = new SceneObject( SpawnPoint );
	%spawn.position = %xPos SPC %yPos;
	
	gameScene.add( %spawn );

	%spawn.spawnTimer = 180000;
	%spawn.startTimer( spawnEnemy, %spawn.spawnTimer , 0 );
}

//I ran out of time to fix this oh well! don't pause a lot I guess!
function SpawnPoint::reactivateTimer( %this ) {
	if ( %this.spawnTimer > 10000 ) {
		%this.spawnTimer -= 10000;
	}
	%this.startTimer( spawnEnemy, %this.spawntimer, 0 );
}

function SpawnPoint::spawnEnemy( %this ) {
	if ( %this.spawnTimer > 60000 ) {
		%this.spawnTimer -= 60000;
	} else if ( %this.spawnTimer > 10000 ) { 
		%this.spawnTimer -= 10000;
	}
	%enemy = createEnemyShip();
	%enemy.position = %this.getPosition();

	GameScene.add( %enemy );

	%enemy.attackMother();
}
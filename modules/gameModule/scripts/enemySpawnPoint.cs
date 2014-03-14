function createEnemySpawnPoint( %xPos, %yPos ) {
	%spawn = new SceneObject( SpawnPoint );
	%spawn.position = %xPos SPC %yPos;
	
	gameScene.add( %spawn );

	%spawn.startTimer( spawnEnemy, 1000000, 0 );
}

function SpawnPoint::reactivateTimer( %this ) {
	%this.startTimer( spawnEnemy, 10000000, 0 );
}

function SpawnPoint::spawnEnemy( %this ) {
	%enemy = createEnemyShip();
	%enemy.position = %this.getPosition();

	GameScene.add( %enemy );

	%enemy.attackMother();
}
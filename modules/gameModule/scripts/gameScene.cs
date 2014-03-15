function createScene() {
		if ( isObject( GameScene ) ) {
				destroyScene();
		}
		
		new Scene( GameScene );
}

function destroyScene() { 
		if ( !isObject( GameScene ) ) {
				return; 
		}
		
		scene.delete();
}

function createPlayerUI() {
	%playerUIWindow = new SceneWindow( UIWindow ) {
		Profile = GuiDefaultProfile;
		size = "100 75";
		sosition = "0 0";
	};

	%playerUI = new Scene( UIScene );

	%health = new Sprite( HealthBar ) {
		image = "gameModule:healthBar";
		size = "48 1.4";
		position = "-0.2 -34";
		SceneLayer = 0;
	};
	%playerUI.add( %health );

	%healthGraphic = new Sprite() {
		image = "gameModule:healthGraphic";
		size = "51 2.5";
		position = "0 -34";
		SceneLayer = 1;
	};
	%playerUI.add( %healthGraphic );

	%fuel = new Sprite( FuelBar ) {
		image = "gameModule:fuelBar";
		size = "0 1.5";
		position = "-45 29";
		SceneLayer = 0;
		angle = 90;
	};
	%fuel.amount = 0;
	%fuel.maxSize = "11.3";
	%playerUI.add( %fuel );

	%fuelBack = new Sprite() {
		image = "gameModule:healthGraphic";
		size = "12 2.5";
		position = "-45 29";
		SceneLayer = 1;
		angle = 90;
	};
	%playerUI.add( %fuelBack );

	%fuelLabel = new ImageFont() {
		image = "gameModule:font";
		FontSize = "2";
		text = "FUEL";
		position = "-45 21";
		SceneLayer = 0;
	};
	%playerUI.add( %fuelLabel );

	%playerUIWindow.setScene( %playerUI );
	Canvas.add( %playerUIWindow );
}

function HealthBar::updateHealth( %this, %change ) {
	Ship.health = Ship.health - %change;
	%currentHealth = Ship.health;
	%size = %currentHealth / 100;
	%sizeX = %size * %this.getWidth();

	%x = getWord( %this.getPosition, 0 ) - ( ( 48 - %sizeX ) / 2 );

	%this.setPositionX( %x );
	%this.setSizeX( %sizeX );
}

function FuelBar::updateFuel( %this ) {
	if ( %this.amount < 4 ) {
		%this.amount += 1;
		%size = %this.amount / 4;
		%sizeX = %size * %this.maxSize;

		%y = getWord( %this.getPosition(), 1 ) - ( ( %this.maxSize  - %sizeX ) / 2 );
		%this.setPositionY( %y );
		%this.setSizeX( %sizeX ); 
		echo( %this.getPosition() );
	}
}
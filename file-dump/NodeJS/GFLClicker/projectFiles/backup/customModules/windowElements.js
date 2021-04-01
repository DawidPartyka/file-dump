function Coordinate(x, y){
  this.x = x;
  this.y = y;
}

class Clickable{
  constructor(id, tl, br, h, w) {
    this.id = id;
    this.position = {
      tl: tl,
      br: br
    };
    this.height = h;
    this.width = w;
    this.validationData = [];
  }
  getClickPos(){
    const short = this.position;
    return new Coordinate(Math.ceil(short.tl.x + this.width/2), Math.ceil(short.tl.y + this.height/2));
  }
  validateClickPos(){
    //..TODO: Collect some pixels and compare with this.validationData[]
  }
}

class UniqueButton extends Clickable{
  constructor(windowVal, marLeft, marTop, vW, vH){
    const width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(0, tl, br, height, width)
  }
}

class LogisticMission extends Clickable{
  constructor(x, windowVal){
    const vH      = 0.08557,
          vW      = 0.121354,
          marLeft = 0.32173,
          marTop  = 0.870705,
          width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          gap     = 0.041392 * windowVal.width,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width + (width + gap) * x * 1.1, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(x, tl, br, height, width);
  }
}

class LogisticChapter extends Clickable{
  constructor(windowVal, marLeft, marTop, vW, vH){
    const width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(0, tl, br, height, width)
  }
  scrollToChapter(x){
    //..TODO: Scroll as many chapters down as given in 'x' than after all the actions scroll back up (maybe even a bit more)
  }
}

class Window{
  constructor(tlx, tly, brx, bry) {
    this.position = {
      tl: new Coordinate(tlx, tly),
      br: new Coordinate(brx, bry)
    };
    this.height = bry - tly;
    this.width = brx - tlx;
    this.basicUnit = this.height/this.width;

    this.contents = {
      logistics: {
        main: {},
        chapter: {},
        mission: [],
        confirmEchelon: {},
        repeat_mission: {}
      },
      unique:{
        combat: {}
      }
    };
  }
  init(){
    this.contents.unique.combat             = new UniqueButton(this, 0.632173, 0.609060, 0.187206, 0.127516);
    this.contents.logistics.main            = new UniqueButton(this, 0.007246, 0.278523, 0.129821, 0.098993);
    this.contents.logistics.confirmEchelon  = new UniqueButton(this, 0.849482, 0.857382, 0.127939, 0.082214);
    this.contents.logistics.chapter         = new LogisticChapter(this, 0.144873, 0.144295, 0.098777, 0.115771);
    //this.contents.logistics.chapter = new LogisticChapter(this, 0.295390, 0.139261, 0.096895, 0.112416);

    for(let i = 0; i < 4; i++){
      this.contents.logistics.mission.push(new LogisticMission(i, this));
    }
  }
}

module.exports = Window;

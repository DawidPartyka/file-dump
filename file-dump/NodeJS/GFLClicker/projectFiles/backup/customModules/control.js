class Routes{
  constructor(obj) {
    this.routeTo   = obj.To;
    this.routeBack = obj.Back;
  }
  completeTasks(delay){
    //.. Functions making a proper call to the API
    async function callScroll(action){
      const pos = action.getClickPos();
      robot.moveMouse(pos.x, pos.y);

      const promise = new Promise((resolve, rej) => {
        for(let i = 0, i < action.num; i++){
          setTimeout(function(){
            robot.moveMouse(0, 0);
            robot.mouseToggle("down");
            robot.dragMouse(100, 100);
            robot.mouseToggle("up");

            resolve(true);
          }, delay/10)                      //Waiting the given amount of time to 'Click'
        }

        robot.mouseClick();
        resolve(true);
      });

      let fin = await promise;              //Wait for the resolve of the promise
    }

    async function call(action){
      const pos = action.getClickPos();
      robot.moveMouse(pos.x, pos.y);

      const promise = new Promise((resolve, rej) => {
        setTimeout(function(){
          robot.mouseClick();
          resolve(true);
        }, delay)                           //Waiting the given amount of time to 'Click'
      });

      let fin = await promise;              //Wait for the resolve of the promise
    }

    function recurseCalls(actionTo, actionBack){  //Make one call. Wait for response. Shift data. Recurses till list is not empty.
      if(actionTo.length){                        //Sanity check
        if(actionTo[0].className === LogisticChapter){
          callScroll(actionTo[0]).then(function(){
            actionTo.shift();
            recurseCalls(actionTo);
          });
        }
        else{
          call(actionTo[0]).then(function(){
            actionTo.shift();
            recurseCalls(actionTo);
          });
        }

      }
      else if(actionBack.length){                 //Sanity check
        call(actionBack[0]).then(function(){
          actionBack.shift();
          recurseCalls(actionBack);
        });
      }
      else{
        //..All finished. Go to sleep or smth.
      }
    }

    let copyRouteTo = this.routeTo;
    let copyRouteBack = this.routeBack;
    recurseCalls(copyRouteTo, copyRouteBack);
  }
}

class StartLogisticMission extends Routes{
  constructor(chapter, mission, Win){ //Pass the windowElements to the constructor pls!
    const short = Window.contents.logistics;
    short.chapter.num = chapter;
    let rT = [Win.contents.unique.combat, short.main, short.chapter, short.mission[mission], short.confirmEchelon];
  }
}

/*class Action{
  constructor(obj) {
    this.act          = obj.action;  //Type of action
    this.echlon       = obj.echelon; //Which echelon is it. Just for information
    this.timeOfAction = obj.time;    //When to start the actions
    this.actionDelay  = 5000;        //Time between actions for everything to load
  }
}*/

let control = {
    queue: [],
    Win: {},
    sortQueue: function(){

    },
    createQueue: function(arr, Win){
      this.Win = Win; //Stor windowElements class
      let spareArr = [];
      while(arr){
        const data = arr[0].mission.split('-');
        spareArr.push({time: arr[0].time, chapter: data[0], mission: data[1]});
      }

      this.initMissions(spareArr);
    },
    initMissions: function(){
      async function call(action, delay){
        const pos = action.getClickPos();
        robot.moveMouse(pos.x, pos.y);

        const promise = new Promise((resolve, rej) => {
          setTimeout(function(){
            robot.mouseClick();
            resolve(true);
          }, delay)                           //Waiting the given amount of time to 'Click'
        });

        let fin = await promise;              //Wait for the resolve of the promise
      }

      const short = this.Win.contents.logistics;
      const basicDelay = 5000;

      function goThroughQueue(q){
        const cur = q[0];

        call(this.Win.contents.unique.combat, basicDelay).then(function(){
          call(short.main, basicDelay).then(function(){
            callDrag(parseInt(cur.chapter, 10), 'down').then(function(){
              call(short.mission[parseInt(cur.mission, 10)]).then(function(){
                callDrag(parseInt(cur.chapter, 10), 'up').then(function(){
                  /*
                  ...Somehow do another calls to go out to main screen and add the current shit to queue with a correct time.
                  In the end sort everything. Make something to wakeup to repeat missions.
                  DO THE FUCKIN DRAGGING CORRECTLY U PIECE OF SHIT
                  */
                });
              });
            });
          });
        });
      }

    }
}

module.exports = control;

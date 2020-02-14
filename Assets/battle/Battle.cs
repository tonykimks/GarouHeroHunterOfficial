using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Battle : MonoBehaviour
{
    public Character garou;
    public Character enemy;
    private BattleState battleState;
    private enum BattleState
    {
        Start,
        WhatWillYouDo,
        Fight,
        Aim,
        Battle,
        Run,
        Win,
        Lose
    }

    private CursorLocation cursorLocation;
    private enum CursorLocation
    {
        Fight,
        Run,
        Move1,
        Move2,
        Move3,
        Move4
    }

    public GameObject expositionText;
    public GameObject cursor;
    public GameObject fightText;
    public GameObject runText;
    public GameObject move1Text;
    public GameObject move2Text;
    public GameObject move3Text;
    public GameObject move4Text;
    public GameObject targetLine;
    public GameObject targetCenter;
    public GameObject target;
    public GameObject aimText;
    public GameObject enemyHealth;
    public GameObject enemyEnergy;
    public GameObject garouHealth;
    public GameObject garouEnergy;
    public GameObject playerNameText;
    public GameObject playerLevelText;
    public GameObject enemyNameText;
    public GameObject enemyLevelText;

    private bool battleSequence;

    private float enemyBarStartWidth = 72.17f;
    private float playerBarStartWidth = 81.39f;

    // Start is called before the first frame update
    void Start()
    {
        Stat garouStats = new Stat(1000, 109, 146, 116, 136, 136, 216);
        Move[] garouMoves = new Move[] { Resources.waterStreamFist, Resources.poisonPowder, Resources.crossChop, Resources.bulkUp };
        Stat enemyStats = new Stat(890, 150, 156, 124, 116, 144, 146);
        Move[] enemyMoves = new Move[] { Resources.machineGunBlows, Resources.bulkUp, Resources.poisonPowder };
        Dictionary<int, Move> garouAllMoves = new Dictionary<int, Move>();
        this.garou = new Character(Resources.Type.Fight, Resources.Type.None, "Garou", Resources.Rank.None, 25, garouStats, garouMoves, garouAllMoves, Resources.Status.Healthy);
        this.enemy = new Character(Resources.Type.Electric, Resources.Type.None, "Genos", Resources.Rank.S, 25,  enemyStats, enemyMoves, garouAllMoves, Resources.Status.Healthy);

        this.battleState = BattleState.Start;
        this.cursorLocation = CursorLocation.Fight;
        populateMoves();
        this.battleSequence = false;

        this.playerNameText.GetComponent<UnityEngine.UI.Text>().text = this.garou.getName();
        this.playerLevelText.GetComponent<UnityEngine.UI.Text>().text = "Lv" + this.garou.getLevel().ToString();
        this.enemyNameText.GetComponent<UnityEngine.UI.Text>().text = this.enemy.getName();
        this.enemyLevelText.GetComponent<UnityEngine.UI.Text>().text = "Lv" + this.enemy.getLevel().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        setCursorPosition();
        setupPanelGUI();

        if (this.battleState == BattleState.Start)
        {
            // animate the two characters arriving on screen
            // set text to "a hero appeared!"
            // transition to next WhatWillYouDo state
            string introText = "You found the " + this.enemy.getRank().ToString() + " class hero, " + this.enemy.getName() + "!";
            setExpositionText(introText);
            if ( Input.GetKeyDown( KeyCode.Z ) || Input.GetKeyDown( KeyCode.X ) )
            {
                this.battleState = BattleState.WhatWillYouDo;
            }

        } else if (this.battleState == BattleState.WhatWillYouDo)
        {
            // show side menu with options of FIGHT and RUN
            // set text to "what will you do?"
            // upon menu selection, transition to appropriate state
            setExpositionText("What will you do?");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if ( this.cursorLocation == CursorLocation.Fight )
                {
                    this.cursorLocation = CursorLocation.Move1;
                    this.battleState = BattleState.Fight;
                } else if ( this.cursorLocation == CursorLocation.Run )
                {
                    this.battleState = BattleState.Run;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.cursorLocation = CursorLocation.Fight;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.cursorLocation = CursorLocation.Run;
            }
        } else if (this.battleState == BattleState.Fight)
        {
            // display moves in center menu
            // upon move selection, transition to aim state (if necessary)

            int numMoves = this.garou.getMoves().Length;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (this.cursorLocation == CursorLocation.Move3)
                {
                    this.cursorLocation = CursorLocation.Move1;
                }

                if (this.cursorLocation == CursorLocation.Move4)
                {
                    this.cursorLocation = CursorLocation.Move2;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (this.cursorLocation == CursorLocation.Move1)
                {
                    if (numMoves >= 3)
                    {
                        this.cursorLocation = CursorLocation.Move3;
                    }
                   
                }

                if (this.cursorLocation == CursorLocation.Move2)
                {
                    if (numMoves == 4)
                    {
                        this.cursorLocation = CursorLocation.Move4;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (this.cursorLocation == CursorLocation.Move2)
                {
                    this.cursorLocation = CursorLocation.Move1;
                }

                if (this.cursorLocation == CursorLocation.Move4)
                {
                    this.cursorLocation = CursorLocation.Move3;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (this.cursorLocation == CursorLocation.Move1)
                {
                    if (numMoves >= 2)
                    {
                        this.cursorLocation = CursorLocation.Move2;
                    }
                }

                if (this.cursorLocation == CursorLocation.Move3)
                {
                    if (numMoves == 4)
                    {
                        this.cursorLocation = CursorLocation.Move4;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                this.battleState = BattleState.Aim;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                this.battleState = BattleState.WhatWillYouDo;
                this.cursorLocation = CursorLocation.Fight;
            }
        } else if (this.battleState == BattleState.Aim)
        {
            // display aiming mechanism
            // upon aiming, transition to battle state
            setupAimTarget();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // should make a check for if using move is valid
                // (cost? situation?)
                this.battleState = BattleState.Battle;
                this.battleSequence = true;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                this.battleState = BattleState.Fight;
            }
        } else if (this.battleState == BattleState.Battle)
        {
            // miss/paralyze/move for first attacker
            // miss/paralyze/move for second attacker
            // status damage for opponent
            // status damage for player
            if (battleSequence)
            {
                StartCoroutine(battle2());
            }
        } else if (this.battleState == BattleState.Run)
        {
            // if health is above 50%, switch text to "you ran away" and end battle
            // if health is lower than 50%, switch text to "run failed" and transition to WhatWillYouDo state
            string endText = "Ran away safely!";
            setExpositionText(endText);
        } else if (this.battleState == BattleState.Win)
        {
            // Text is "you defeated .." 
            // apply experience bonus and end battle
            string endText = "You have successfully hunted " + this.enemy.getName() + "!";
            setExpositionText(endText);
        } else if (this.battleState == BattleState.Lose)
        {
            // Text is "you were defeated.." "the hero ranks up for defeating the hero hunter!"
            // end battle
            string endText = "You have been defeated by " + this.enemy.getName() + "...";
            setExpositionText(endText);
        }
    }

    private void setExpositionText( string text)
    {
        this.expositionText.GetComponent<UnityEngine.UI.Text>().text = text;
    }

    private void setupPanelGUI()
    {
        if (this.battleState == BattleState.Start)
        {
            setActiveMoves(false);
            this.expositionText.SetActive(true);
            this.setActiveTargets(false);
            this.aimText.SetActive(false);
            setActiveSideOptions(false);
        }
        else if (this.battleState == BattleState.WhatWillYouDo)
        {
            this.expositionText.SetActive(true);
            setActiveMoves(false);
            this.aimText.SetActive(false);
            setActiveSideOptions(true);
        }
        else if (this.battleState == BattleState.Fight)
        {
            this.expositionText.SetActive(false);
            setActiveMoves(true);
            this.setActiveTargets(false);
            this.aimText.SetActive(false);
            setActiveSideOptions(false);

        }
        else if (this.battleState == BattleState.Aim)
        {
            this.setActiveTargets(true);
            setActiveMoves(false);
            this.aimText.SetActive(true);
        }

        else if (this.battleState == BattleState.Battle)
        {
            this.expositionText.SetActive(true);
            this.setActiveTargets(false);
        } 
        else if (this.battleState == BattleState.Run)
        {

        }
        else if (this.battleState == BattleState.Win)
        {

        }
        else if (this.battleState == BattleState.Lose)
        {

        }
    }

    private void setCursorPosition()
    {
        if (this.battleState == BattleState.WhatWillYouDo || this.battleState == BattleState.Fight)
        {
            this.cursor.SetActive(true);
        }
        else
        {
            this.cursor.SetActive(false);
        }

        if (this.cursorLocation == CursorLocation.Fight)
        {
            this.cursor.transform.position = this.fightText.transform.position;
        } else if (this.cursorLocation == CursorLocation.Run)
        {
            this.cursor.transform.position = this.runText.transform.position;
        } else if (this.cursorLocation == CursorLocation.Move1)
        {
            this.cursor.transform.position = this.move1Text.transform.position;
        } else if (this.cursorLocation == CursorLocation.Move2)
        {
            this.cursor.transform.position = this.move2Text.transform.position;
        } else if (this.cursorLocation == CursorLocation.Move3)
        {
            this.cursor.transform.position = this.move3Text.transform.position;
        } else if (this.cursorLocation == CursorLocation.Move4)
        {
            this.cursor.transform.position = this.move4Text.transform.position;
        }
    }

    private void setActiveMoves( bool active)
    {
        this.move1Text.SetActive(active);
        this.move2Text.SetActive(active);
        this.move3Text.SetActive(active);
        this.move4Text.SetActive(active);
    }

    private void setActiveSideOptions( bool active)
    {
        this.fightText.SetActive(active);
        this.runText.SetActive(active);
    }

    private void setActiveTargets( bool active )
    {
        this.target.SetActive(active);
        this.targetLine.SetActive(active);
        this.targetCenter.SetActive(active);
    }

    private void populateMoves()
    {
        this.move1Text.GetComponent<UnityEngine.UI.Text>().text = this.garou.getMoves()[0].getName();
        if (this.garou.getMoves().Length >= 2)
        {
            this.move2Text.GetComponent<UnityEngine.UI.Text>().text = this.garou.getMoves()[1].getName();
        }
        else
        {
            this.move2Text.GetComponent<UnityEngine.UI.Text>().text = "--";
        }

        if (this.garou.getMoves().Length >= 3)
        {
            this.move3Text.GetComponent<UnityEngine.UI.Text>().text = this.garou.getMoves()[2].getName();
        }
        else
        {
            this.move3Text.GetComponent<UnityEngine.UI.Text>().text = "--";
        }

        if (this.garou.getMoves().Length == 4)
        {
            this.move4Text.GetComponent<UnityEngine.UI.Text>().text = this.garou.getMoves()[3].getName();
        }
        else
        {
            this.move4Text.GetComponent<UnityEngine.UI.Text>().text = "--";
        }

    }

    private Move getSelectedMove()
    {
        Move[] moves = this.garou.getMoves();
        if (this.cursorLocation == CursorLocation.Move1)
        {
            return moves[0];
        } else if (this.cursorLocation == CursorLocation.Move2)
        {
            return moves[1];
        }
        else if (this.cursorLocation == CursorLocation.Move3)
        {
            return moves[2];
        }
        // on move4
        return moves[3];
    }

    private Move getEnemyMove()
    {
        System.Random rnd = new System.Random();
        int numMoves = this.enemy.getMoves().Length;
        int moveIndex = rnd.Next(0, numMoves);
        return this.enemy.getMoves()[moveIndex];
    }

    private float getTrueAccuracy()
    {
        Move selectedMove = getSelectedMove();

        float moveAccuracy = selectedMove.getAccuracy() / 100f;

        float speedDifference = this.garou.getCurrentStats().getSpd() - this.enemy.getCurrentStats().getSpd();

        if (speedDifference < -150f)
        {
            speedDifference = -150f;
        }
        else if (speedDifference > 150f)
        {
            speedDifference = 150f;
        }

        float sdp = (speedDifference + 150f) / 300f;

        float trueAccuracy = ((20f * sdp) + (80 * moveAccuracy)) / 100f;
        return trueAccuracy;
    }

    private float getEnemyAccuracy(Move move)
    {
        float moveAccuracy = move.getAccuracy() / 100f;

        float speedDifference = this.enemy.getCurrentStats().getSpd() - this.garou.getCurrentStats().getSpd();

        if (speedDifference < -150f)
        {
            speedDifference = -150f;
        }
        else if (speedDifference > 150f)
        {
            speedDifference = 150f;
        }

        float sdp = (speedDifference + 150f) / 300f;

        float trueAccuracy = ((20 * sdp) + (80 * moveAccuracy)) / 100f;
        return trueAccuracy;
    }

    private void setupAimTarget()
    {
        float accuracy = getTrueAccuracy();
        float targetSpeed = 5f - (4.5f * accuracy);

        float width = this.targetLine.GetComponent<RectTransform>().rect.width;
        float start = this.targetLine.transform.position.x - (width / 2f);
        float end = start + width;
        float middle = (start + end) / 2f;

        this.target.transform.position = new Vector3(Mathf.Lerp(start, end, Mathf.PingPong(Time.time * targetSpeed, 1)), this.target.transform.position.y, this.target.transform.position.z);
    }

    private void reduceEnemyHealth(int amount)
    {
        Stat enemyCurrStats = this.enemy.getCurrentStats();
        int enemyTotalHp = this.enemy.getOriginalStats().getHp();
        int enemyCurrentHp;
        if (amount >= enemyCurrStats.getHp())
        {
            enemyCurrentHp = 0;
        }
        else
        {
            enemyCurrentHp = enemyCurrStats.getHp() - amount;
        }
        this.enemy.setCurrentStats(enemyCurrentHp, enemyCurrStats.getEnergy(), enemyCurrStats.getAtk(), enemyCurrStats.getSpatk(), enemyCurrStats.getDef(), enemyCurrStats.getSpdef(), enemyCurrStats.getSpd());

        float hpRatio = enemyCurrentHp / (float)enemyTotalHp;
        float newWidth = this.enemyBarStartWidth * hpRatio;

        print(amount);
        print(enemyCurrentHp);
        print(hpRatio);

        RectTransform enemyHpBarRect = this.enemyHealth.GetComponent<RectTransform>();
        enemyHpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private void reduceGarouHealth(int amount)
    {
        Stat garouCurrStats = this.garou.getCurrentStats();
        int garouTotalHp = this.garou.getOriginalStats().getHp();
        int garouCurrentHp;
        if (amount >= garouCurrStats.getHp())
        {
            garouCurrentHp = 0;
        }
        else
        {
            garouCurrentHp = garouCurrStats.getHp() - amount;
        }
        this.garou.setCurrentStats(garouCurrentHp, garouCurrStats.getEnergy(), garouCurrStats.getAtk(), garouCurrStats.getSpatk(), garouCurrStats.getDef(), garouCurrStats.getSpdef(), garouCurrStats.getSpd());

        float hpRatio = garouCurrentHp / (float)garouTotalHp;
        float newWidth = this.playerBarStartWidth * hpRatio;

        RectTransform garouHpBarRect = this.garouHealth.GetComponent<RectTransform>();
        garouHpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

    }

    IEnumerator battle2()
    {
        this.battleSequence = false;

        // print accuracy of move
        float diff = Math.Abs(this.targetCenter.transform.position.x - this.target.transform.position.x);
        float accuracy = (1f - (diff / (this.targetLine.GetComponent<RectTransform>().rect.width / 2f))) * 100f;
        this.aimText.GetComponent<UnityEngine.UI.Text>().text = accuracy.ToString() + "%";

        // determine who attacks first
        Character firstAttacker;
        Character secondAttacker;
        Move firstMove;
        Move secondMove;
        string firstName;
        string secondName;
        float firstAccuracy;
        float secondAccuracy;
        if (this.garou.getCurrentStats().getSpd() > this.enemy.getCurrentStats().getSpd())
        {
            firstAttacker = this.garou;
            secondAttacker = this.enemy;

            firstMove = getSelectedMove();
            secondMove = getEnemyMove();

            firstName = "Garou";
            secondName = this.enemy.getName();

            firstAccuracy = accuracy;
            System.Random rnd = new System.Random();
            secondAccuracy = rnd.Next(1, 100);
        }

        else
        {
            firstAttacker = this.enemy;
            secondAttacker = this.garou;

            firstMove = getEnemyMove();
            secondMove = getSelectedMove();

            firstName = this.enemy.getName();
            secondName = "Garou";

            System.Random rnd = new System.Random();
            firstAccuracy = rnd.Next(1, 100);
            secondAccuracy = accuracy;
        }

        // first attacker loses energy
        reduceEnergy(firstAttacker, firstMove.getCost(), firstMove.getPenalty());

        // end match if first attacker dies on energy cost
        if (setWinOrLose())
        {
            yield break;
        }

        // first move
        setExpositionText(firstName + " used " + firstMove.getName() + "!");
        yield return new WaitForSeconds(1);

        // determine if first move hits and crit
        bool[] hc = hitAndCrit(firstAttacker, firstMove, firstAccuracy);
        bool firstMoveHits = hc[0];
        bool firstMoveCrits = hc[1];
        float eff = getEffectiveness(firstMove.getType(), secondAttacker);

        // move does not affect target
        if (eff > -0.0001f && eff < 0.001f)
        {
            setExpositionText("Doesn't affect " + secondName + "...");
            yield return new WaitForSeconds(1);
        }
        else
        {
            if (firstMoveHits)
            {
                // hit

                // determine type of first move
                if (firstMove.GetType() == typeof(DamageMove))
                {
                    damageAttack(firstAttacker, firstMove, eff, firstMoveCrits);
                    yield return new WaitForSeconds(1);

                    if (firstMoveCrits)
                    {
                        setExpositionText("It's a critical hit!");
                        yield return new WaitForSeconds(1);
                    }

                    // effectiveness
                    if (eff >= 2.0f)
                    {
                        setExpositionText("It's Super Effective!");
                        yield return new WaitForSeconds(1);
                    }
                    else if (eff <= 0.5f)
                    {
                        setExpositionText("Not very effective...");
                        yield return new WaitForSeconds(1);
                    }

                    // end match if health is 0
                    if (setWinOrLose())
                    {
                        yield break;
                    }
                }
                else if (firstMove.GetType() == typeof(StatusMove))
                {
                    StatusMove firstStatusMove = (StatusMove)firstMove;
                    Resources.Status status = firstStatusMove.getStatus();
                    if (firstStatusMove.getTarget())
                    {
                        // target is enemy
                        if (secondAttacker.getStatus() != Resources.Status.Healthy)
                        {
                            // already has a status ailment
                            setExpositionText(secondName + " is already " + getStatusText(status) + "!");
                            yield return new WaitForSeconds(1);
                        }
                        else
                        {
                            if (firstMoveCrits)
                            {
                                // crit
                                secondAttacker.setStatus(getBadStatus(status));
                                setExpositionText("It's a Crit!");
                                yield return new WaitForSeconds(1);
                                setExpositionText(secondName + " has been " + getStatusText(getBadStatus(status)) + "!");
                                yield return new WaitForSeconds(1);
                            }
                            else
                            {
                                // normal
                                secondAttacker.setStatus(status);
                                setExpositionText(secondName + " has been " + getStatusText(status) + "!");
                                yield return new WaitForSeconds(1);
                            }
                        }

                    }
                    else
                    {
                        // target is self
                    }
                }
                else if (firstMove.GetType() == typeof(StatMove))
                {
                    StatMove firstStatMove = (StatMove)firstMove;
                    Resources.StatType[] stats = firstStatMove.getStats();
                    int f = firstStatMove.getFactor();
                    if (firstMoveCrits)
                    {
                        f++;
                        setExpositionText("It's a Crit!");
                        yield return new WaitForSeconds(1);
                    }
                    foreach (Resources.StatType stat in stats)
                    {
                        if (firstStatMove.getTarget())
                        {
                            // target is enemy
                            if (secondAttacker.changeStat(stat, f))
                            {
                                // stat already max or min
                                if (f > 0)
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " won't go any higher!");
                                }
                                else
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " can't go any lower!");
                                }
                            }
                            else
                            {
                                // stat can increase or decrease
                                if (f > 0)
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " increased by " + f.ToString() + "!");
                                }
                                else
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " decreased by " + f.ToString() + "!");
                                }
                            }
                        }
                        else
                        {
                            // target is self
                            if (firstAttacker.changeStat(stat, f))
                            {
                                // stat is already max or min
                                if (f > 0)
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " won't go any higher!");
                                }
                                else
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " can't go any lower!");
                                }
                            }
                            else
                            {
                                // stat can increase or decrease
                                if (f > 0)
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " increased by " + f.ToString() + "!");
                                }
                                else
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " decreased by " + f.ToString() + "!");
                                }
                            }
                        }
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            else
            {
                // miss
                if (firstMove.GetType() == typeof(DamageMove))
                {
                    setExpositionText(firstName + "'s attack missed...");
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    setExpositionText(firstName + " failed...");
                    yield return new WaitForSeconds(1);
                }
            }

            this.cursorLocation = CursorLocation.Fight;
        }

        // second attacker loses energy
        reduceEnergy(secondAttacker, secondMove.getCost(), secondMove.getPenalty());

        // end match if first attacker dies on energy cost
        if (setWinOrLose())
        {
            yield break;
        }

        // second move
        setExpositionText(secondName + " used " + secondMove.getName() + "!");
        yield return new WaitForSeconds(1);

        // determine if second move hits and crit
        bool[] hc2 = hitAndCrit(secondAttacker, secondMove, secondAccuracy);
        bool secondMoveHits = hc2[0];
        bool secondMoveCrits = hc2[1];
        float eff2 = getEffectiveness(secondMove.getType(), firstAttacker);

        // move does not affect target
        if (eff2 > -0.0001f && eff2 < 0.001f)
        {
            setExpositionText("Doesn't affect " + firstName + "...");
            yield return new WaitForSeconds(1);
        }
        else
        {
            if (secondMoveHits)
            {
                // hit

                // determine type of first move
                if (secondMove.GetType() == typeof(DamageMove))
                {
                    damageAttack(secondAttacker, secondMove, eff2, secondMoveCrits);
                    yield return new WaitForSeconds(1);

                    if (secondMoveCrits)
                    {
                        setExpositionText("It's a critical hit!");
                        yield return new WaitForSeconds(1);
                    }

                    // effectiveness
                    if (eff2 >= 2.0f)
                    {
                        setExpositionText("It's Super Effective!");
                        yield return new WaitForSeconds(1);
                    }
                    else if (eff2 <= 0.5f)
                    {
                        setExpositionText("Not very effective...");
                        yield return new WaitForSeconds(1);
                    }

                    // end match if health is 0
                    if (setWinOrLose())
                    {
                        yield break;
                    }
                }
                else if (secondMove.GetType() == typeof(StatusMove))
                {
                    StatusMove secondStatusMove = (StatusMove)secondMove;
                    Resources.Status status = secondStatusMove.getStatus();
                    if (secondStatusMove.getTarget())
                    {
                        // target is enemy
                        if (firstAttacker.getStatus() != Resources.Status.Healthy)
                        {
                            // already has a status ailment
                            setExpositionText(firstName + " is already " + getStatusText(status) + "!");
                            yield return new WaitForSeconds(1);
                        }
                        else
                        {
                            if (secondMoveCrits)
                            {
                                // crit
                                firstAttacker.setStatus(getBadStatus(status));
                                setExpositionText("It's a Crit!");
                                yield return new WaitForSeconds(1);
                                setExpositionText(firstName + " has been " + getStatusText(getBadStatus(status)) + "!");
                                yield return new WaitForSeconds(1);
                            }
                            else
                            {
                                // normal
                                firstAttacker.setStatus(status);
                                setExpositionText(firstName + " has been " + getStatusText(status) + "!");
                                yield return new WaitForSeconds(1);
                            }
                        }
                    }
                    else
                    {
                        // target is self
                    }
                }
                else if (secondMove.GetType() == typeof(StatMove))
                {
                    StatMove secondStatMove = (StatMove)secondMove;
                    Resources.StatType[] stats = secondStatMove.getStats();
                    int f = secondStatMove.getFactor();
                    if (secondMoveCrits)
                    {
                        f++;
                        setExpositionText("It's a Crit!");
                        yield return new WaitForSeconds(1);

                    }
                    foreach (Resources.StatType stat in stats)
                    {
                        if (secondStatMove.getTarget())
                        {
                            // target is enemy
                            if (firstAttacker.changeStat(stat, f))
                            {
                                // stat already max or min
                                if (f > 0)
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " won't go any higher!");
                                }
                                else
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " can't go any lower!");
                                }
                            }
                            else
                            {
                                // stat can increase or decrease
                                if (f > 0)
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " increased by " + f.ToString() + "!");
                                }
                                else
                                {
                                    setExpositionText(firstName + "'s " + stat.ToString() + " decreased by " + f.ToString() + "!");
                                }
                            }
                        }
                        else
                        {
                            // target is self
                            if (secondAttacker.changeStat(stat, f))
                            {
                                // stat is already max or min
                                if (f > 0)
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " won't go any higher!");
                                }
                                else
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " can't go any lower!");
                                }
                            }
                            else
                            {
                                // stat can increase or decrease
                                if (f > 0)
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " increased by " + f.ToString() + "!");
                                }
                                else
                                {
                                    setExpositionText(secondName + "'s " + stat.ToString() + " decreased by " + f.ToString() + "!");
                                }
                            }
                        }
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            else
            {
                // miss
                if (secondMove.GetType() == typeof(DamageMove))
                {
                    setExpositionText(secondName + "'s attack missed...");
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    setExpositionText(secondName + " failed...");
                    yield return new WaitForSeconds(1);
                }
            }
        }

        // apply poison / burn damage
        float poison = 0.06f;
        float burn = 0.04f;
        float badlyPoison = 0.1f;
        float badlyBurn = 0.06f;
        int garouTotalHealth = this.garou.getOriginalStats().getHp();
        int enemyTotalHealth = this.enemy.getOriginalStats().getHp();
        Resources.Status garouStatus = this.garou.getStatus();
        Resources.Status enemyStatus = this.enemy.getStatus();

        // apply garou damage for status
        if (garouStatus == Resources.Status.Poison)
        {
            reduceHealth(this.garou, (int)Math.Round((float)garouTotalHealth * poison));
            setExpositionText("Garou is hurt by poison!");
            yield return new WaitForSeconds(1);
        } 
        else if (garouStatus == Resources.Status.Burn)
        {
            reduceHealth(this.garou, (int)Math.Round((float)garouTotalHealth * burn));
            setExpositionText("Garou is hurt by its burn!");
            yield return new WaitForSeconds(1);
        }
        else if (garouStatus == Resources.Status.BadlyPoison)
        {
            reduceHealth(this.garou, (int)Math.Round((float)garouTotalHealth * badlyPoison));
            setExpositionText("Garou is hurt by poison!");
            yield return new WaitForSeconds(1);
        }
        else if (garouStatus == Resources.Status.BadlyBurn)
        {
            reduceHealth(this.garou, (int)Math.Round((float)garouTotalHealth * badlyBurn));
            setExpositionText("Garou is hurt by its burn!");
            yield return new WaitForSeconds(1);
        }

        // end match if garou dies on status damage
        if (setWinOrLose())
        {
            yield break;
        }

        // apply enemy damage for status
        if (enemyStatus == Resources.Status.Poison)
        {
            reduceHealth(this.enemy, (int)Math.Round((float)enemyTotalHealth * poison));
            setExpositionText(this.enemy.getName() + " is hurt by poison!");
            yield return new WaitForSeconds(1);
        }
        else if (enemyStatus == Resources.Status.Burn)
        {
            reduceHealth(this.enemy, (int)Math.Round((float)enemyTotalHealth * burn));
            setExpositionText(this.enemy.getName() + " is hurt by its burn!");
            yield return new WaitForSeconds(1);
        }
        else if (enemyStatus == Resources.Status.BadlyPoison)
        {
            reduceHealth(this.enemy, (int)Math.Round((float)enemyTotalHealth * badlyPoison));
            setExpositionText(this.enemy.getName() + " is hurt by poison!");
            yield return new WaitForSeconds(1);
        }
        else if (enemyStatus == Resources.Status.BadlyBurn)
        {
            reduceHealth(this.enemy, (int)Math.Round((float)enemyTotalHealth * badlyBurn));
            setExpositionText(this.enemy.getName() + " is hurt by its burn!");
            yield return new WaitForSeconds(1);
        }

        // end match if enemy dies on status damage
        if (setWinOrLose())
        {
            yield break;
        }

        this.battleState = BattleState.WhatWillYouDo;
        this.cursorLocation = CursorLocation.Fight;
    }

    private string getStatusText(Resources.Status status)
    {
        switch(status)
        {
            case Resources.Status.Poison:
                return "poisoned";
            case Resources.Status.Paralyze:
                return "paralyzed";
            case Resources.Status.Burn:
                return "burned";
            case Resources.Status.BadlyBurn:
                return "badly burned";
            case Resources.Status.BadlyPoison:
                return "badly poisoned";
            case Resources.Status.BadlyParalyze:
                return "badly paralyzed";
            default:
                return "";
        }
    }

    private Resources.Status getBadStatus(Resources.Status status)
    {
        switch (status)
        {
            case Resources.Status.Poison:
                return Resources.Status.BadlyPoison;
            case Resources.Status.Paralyze:
                return Resources.Status.BadlyParalyze;
            case Resources.Status.Burn:
                return Resources.Status.BadlyBurn;
            default:
                return Resources.Status.Healthy;
        }
    }

    private bool[] hitAndCrit(Character c, Move m, float accuracy)
    {
        if (c == this.garou)
        {
            bool moveHits = false;
            bool moveCrits = false;
            if (accuracy >= 100f - ((m.getAccuracy() / 100f) * 30f))
            {
                moveHits = true;
                if (accuracy >= 97f)
                {
                    moveCrits = true;
                }
            }
            return new bool[] { moveHits, moveCrits };
        }
        else
        {
            bool moveHits = false;
            bool moveCrits = false;
            if (accuracy >= 100f - (getEnemyAccuracy(m) * 100f))
            {
                moveHits = true;
                if (accuracy >= 97f)
                {
                    moveCrits = true;
                }
            }
            return new bool[] { moveHits, moveCrits };
        }
    }

    private bool setWinOrLose()
    {
        float enemyHealthWidth = this.enemyHealth.GetComponent<RectTransform>().rect.width;
        if (enemyHealthWidth > -0.001f && enemyHealthWidth < 0.001f)
        {
            this.battleState = BattleState.Win;
            return true;
        }

        float garouHealthWidth = this.garouHealth.GetComponent<RectTransform>().rect.width;
        if (garouHealthWidth > -0.001f && garouHealthWidth < 0.001f)
        {
            this.battleState = BattleState.Lose;
            return true;
        }

        return false;
    }

    private void damageAttack(Character c, Move m, float eff, bool crit)
    {
        if (c == this.garou)
        {
            garouAttack(m, eff, crit);
        }
        else
        {
            enemyAttack(m, eff, crit);
        }
    }

    private void reduceHealth(Character c, int amount)
    {
        if (c == this.garou)
        {
            reduceGarouHealth(amount);
        }
        else
        {
            reduceEnemyHealth(amount);
        }
    }

    private void reduceEnergy(Character c, int amount, int penalty)
    {
        if (c == this.garou)
        {
            reduceGarouEnergy(amount, penalty);
        }
        else
        {
            reduceEnemyEnergy(amount, penalty);
        }
    }

    private void garouAttack(Move move, float eff, bool crit)
    {
        DamageMove dmgMove = (DamageMove)move;
        int dmg = dmgMove.getDamage();
        int totalDamage;

        if (dmgMove.getPhysical())
        {
            // physical move
            float garouAtk = this.garou.getCurrentStats().getAtk();
            float enemyDef = this.enemy.getCurrentStats().getDef();
            totalDamage = (int)Math.Round(dmg * (garouAtk / enemyDef));
        }
        else
        {
            // special
            float garouSpatk = this.garou.getCurrentStats().getSpatk();
            float enemySpdef = this.enemy.getCurrentStats().getSpdef();
            totalDamage = (int)Math.Round(dmg * (garouSpatk / enemySpdef));
        }

        totalDamage = (int)Math.Round(totalDamage * eff);

        if (crit)
        {
            totalDamage = (int)Math.Round(totalDamage * 1.5f);
        }

        if (dmgMove.getPhysical())
        {
            if (this.garou.getStatus() == Resources.Status.Burn)
            {
                totalDamage = (int)Math.Round(totalDamage * 0.75f);
            } else if (this.garou.getStatus() == Resources.Status.BadlyBurn)
            {
                totalDamage = (int)Math.Round(totalDamage * 0.5f);
            }
        } 

        reduceEnemyHealth(totalDamage);
        //if (move.GetType() == typeof(DamageMove))
        //{

        //}
        //else if (move.GetType() == typeof(StatusMove))
        //{

        //}
        //else if (move.GetType() == typeof(StatMove))
        //{
        //    StatMove statMove = (StatMove)move;
        //    if (statMove.getTarget())
        //    {
        //        // target is enemy
        //    }
        //    else
        //    {
        //        // target is self
        //        if (statMove.getIncrease())
        //        {
        //            // stats go up
        //            Stat garouCurrStats = this.garou.getCurrentStats();
        //            foreach (Resources.StatType s in statMove.getStats())
        //            {
        //                this.garou.changeStat(s, statMove.getFactor());
        //            }
        //            print("madeit");
        //        }
        //        else
        //        {
        //            // stats go down   
        //        }
        //    }
        //}
    }

    private void enemyAttack(Move move, float eff, bool crit)
    {
        DamageMove dmgMove = (DamageMove)move;
        int dmg = dmgMove.getDamage();
        int totalDamage;

        if (dmgMove.getPhysical())
        {
            // physical move
            float enemyAtk = this.enemy.getCurrentStats().getAtk();
            float garouDef = this.garou.getCurrentStats().getDef();
            totalDamage = (int)Math.Round(dmg * (enemyAtk / garouDef));
        }
        else
        {
            // special
            float enemySpatk = this.enemy.getCurrentStats().getSpatk();
            float garouSpdef = this.garou.getCurrentStats().getSpdef();
            totalDamage = (int)Math.Round(dmg * (enemySpatk / garouSpdef));
        }

        totalDamage = (int)Math.Round(totalDamage * eff);

        if (crit)
        {
            totalDamage = (int)Math.Round(totalDamage * 1.5f);
            //totalDamage *= 2;
        }

        if (dmgMove.getPhysical())
        {
            if (this.enemy.getStatus() == Resources.Status.Burn)
            {
                totalDamage = (int)Math.Round(totalDamage * 0.75f);
            }
            else if (this.enemy.getStatus() == Resources.Status.BadlyBurn)
            {
                totalDamage = (int)Math.Round(totalDamage * 0.5f);
            }
        }

        reduceGarouHealth(totalDamage);
    }

    private void reduceGarouEnergy(int amount, int penalty)
    {
        Stat garouCurrStats = this.garou.getCurrentStats();
        int garouTotalEnergy = this.garou.getOriginalStats().getEnergy();

        // do health damage if no energy left
        if (garouCurrStats.getEnergy()  == 0)
        {
            int garouTotalHp = this.garou.getOriginalStats().getHp();
            int hpPenalty = (int) Math.Round(garouTotalHp * ((float)penalty / 100f));
            reduceGarouHealth(hpPenalty);
        }

        int garouCurrentEnergy;
        if (amount >= garouCurrStats.getEnergy())
        {
            garouCurrentEnergy = 0;
        }
        else
        {
            garouCurrentEnergy = garouCurrStats.getEnergy() - amount;
        }
        this.garou.setCurrentStats(garouCurrStats.getHp(), garouCurrentEnergy, garouCurrStats.getAtk(), garouCurrStats.getSpatk(), garouCurrStats.getDef(), garouCurrStats.getSpdef(), garouCurrStats.getSpd());

        float energyRatio = garouCurrentEnergy / (float)garouTotalEnergy;
        float newWidth = this.playerBarStartWidth * energyRatio;

        RectTransform garouEnergyBarRect = this.garouEnergy.GetComponent<RectTransform>();
        garouEnergyBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private void reduceEnemyEnergy(int amount, int penalty)
    {
        Stat enemyCurrStats = this.enemy.getCurrentStats();
        int enemyTotalEnergy = this.enemy.getOriginalStats().getEnergy();

        // do health damage if no energy left
        if (enemyCurrStats.getEnergy() == 0)
        {
            int enemyTotalHp = this.enemy.getOriginalStats().getHp();
            int hpPenalty = (int)Math.Round(enemyTotalHp * ((float)penalty / 100f));
            reduceEnemyHealth(hpPenalty);
        }
        int enemyCurrentEnergy;
        if (amount >= enemyCurrStats.getEnergy())
        {
            enemyCurrentEnergy = 0;
        }
        else
        {
            enemyCurrentEnergy = enemyCurrStats.getEnergy() - amount;
        }
        this.enemy.setCurrentStats(enemyCurrStats.getHp(), enemyCurrentEnergy, enemyCurrStats.getAtk(), enemyCurrStats.getSpatk(), enemyCurrStats.getDef(), enemyCurrStats.getSpdef(), enemyCurrStats.getSpd());

        float energyRatio = enemyCurrentEnergy / (float)enemyTotalEnergy;
        float newWidth = this.enemyBarStartWidth * energyRatio;

        RectTransform enemyEnergyBarRect = this.enemyEnergy.GetComponent<RectTransform>();
        enemyEnergyBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private float getEffectiveness(Resources.Type moveType, Character t)
    {
        Resources.Effectiveness eff1 = Resources.typeChart[moveType][t.getType1()];

        Resources.Effectiveness eff2 = Resources.Effectiveness.Normal;
        if (t.getType2() != Resources.Type.None)
        {
            eff2 = Resources.typeChart[moveType][t.getType2()];
        }

        if (eff1 == Resources.Effectiveness.None || eff2 == Resources.Effectiveness.None)
        {
            return 0f;
        }

        float result = 1f;

        if (eff1 == Resources.Effectiveness.Super)
        {
            result *= 2f;
        }
        else if (eff1 == Resources.Effectiveness.Weak)
        {
            result *= 0.5f;
        }

        if (eff2 == Resources.Effectiveness.Super)
        {
            result *= 2f;
        }
        else if (eff2 == Resources.Effectiveness.Weak)
        {
            result *= 0.5f;
        }

        return result;
    }
}

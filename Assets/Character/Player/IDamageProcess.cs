    using System.Collections;
    using Cysharp.Threading.Tasks;

    namespace Character.Player
    {
        public interface IDamageProcess
        {   //OnHitStart, End
            public void TakeDamageOneHit(float damage);
        
            // public void TakeDamageLoopHitStart(float damage, float loopTerm);
            // public void TakeDamageLoopHitStop();
            //
            // IEnumerator LoopHit(float damage, float loopTerm);


        }
    }


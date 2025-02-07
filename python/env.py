import argparse
from dataclasses import dataclass

from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.envs.unity_gym_env import UnityToGymWrapper
def main():

    # Se connecter à l'environnement de débogage dans l'éditeur Unity
    print("Connecting to Unity environment... Please, start the environment")
    unity_env = UnityEnvironment(file_name=None, seed=1, side_channels=[], no_graphics=True)
    print("Unity environment connected")
    env = UnityToGymWrapper(unity_env, uint8_visual=False, allow_multiple_obs=True)
    print("Environment created")
    while True:
        ob = env.step(action=(1, 1)) 
        print(f"Observation: {ob}")

    # Récupérer le nom de comportement (behavior)
    group_spec = list(env.group_spec)
    print(f"Behavior name: {group_spec}")

if __name__ == "__main__":
    main()
    